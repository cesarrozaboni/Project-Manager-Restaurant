using Services.ServiceException;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace AppRestaurante.Controllers
{
    public class TableController : Base.BaseController<Services.Repositories.TableRepository>
    {

        #region "Constantes"
        private const string TAB_CADASTRO = "cadastro-tab";
        private const string TAB_EXCLUIR = "excluir-tab";
        #endregion

        #region "Index"
        /// <summary>
        /// Return index view
        /// </summary>
        /// <param name="tab">tab to show</param>
        /// <param name="message">message to show</param>
        /// <returns>view</returns>
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Index(string tab = TAB_CADASTRO, string message = "")
        {
            System.Collections.Generic.IEnumerable<SelectListItem> objSelectListItem;

            try
            {
                var result = await ObjRepository.GetAllTableAsync();

                objSelectListItem = (from item in result.Tables[Domain.Models.TableDomain.TABLE_NAME].AsEnumerable()
                                     select new SelectListItem()
                                     {
                                         Text     = (string) item[Domain.Models.TableDomain.COLUMN_TABLE_DESCRIPTION],
                                         Value    = item[Domain.Models.TableDomain.COLUMN_TABLE_ID].ToString(),
                                         Selected = false
                                     }).ToList();

                objSelectListItem = objSelectListItem.Prepend(new SelectListItem { Text = "--Selecione--", Value = " ", Selected = true });
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(ErrorView), new { Message = ex.Message });
            }

            ViewData["tab"]      = tab;
            ViewData["message"]  = message;
            ViewData["AllItens"] = objSelectListItem;

            return View();
        }

        #endregion

        #region "Insert table"
        /// <summary>
        /// Insert new table in database
        /// </summary>
        /// <param name="model">viewModel with parameters</param>
        /// <returns>view</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> InsertNewTable(ViewModel.TableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                System.Collections.Generic.List<string> errors = GetErrorsModelState();
                return RedirectToAction(nameof(ErrorView), new { Message = string.Join(",", errors) });
            }

            System.Data.DataTable dataTable = Domain.Models.TableDomain.CreateDataTable();
            System.Data.DataRow row         = Domain.Models.TableDomain.CreateDataRow(dataTable, model.TableId, model.TableDescription);
            dataTable.Rows.Add(row);

            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dataTable);

            try
            {
                await ObjRepository.InsertNewTableAsync(ds);
            }
            catch(IntegrityException ex)
            {
                return RedirectToAction(nameof(ErrorView), new { Message = ex.Message });
            }

            return RedirectToAction(nameof(Index), new { tab = TAB_CADASTRO, message = Util.MessageApp.Msg_Operacao_Realizada_Com_Sucesso });
        }
        #endregion

        #region "Delete table"
        /// <summary>
        /// Delete Table from database
        /// </summary>
        /// <param name="model">model with parameter from view</param>
        /// <returns>view</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteTable(ViewModel.TableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                System.Collections.Generic.List<string> errors = GetErrorsModelState();
                return RedirectToAction(nameof(ErrorView), new { Message = string.Join(",", errors) });
            }

            try 
            { 
                await ObjRepository.DeleteTableAsync(model.TableId);
            }
            catch(IntegrityException ex)
            {
                return RedirectToAction(nameof(ErrorView), new { Message = ex.Message});
            }

            return RedirectToAction(nameof(Index), new { tab = TAB_EXCLUIR, message = Util.MessageApp.Msg_Operacao_Realizada_Com_Sucesso });
        }
        #endregion

    }
}