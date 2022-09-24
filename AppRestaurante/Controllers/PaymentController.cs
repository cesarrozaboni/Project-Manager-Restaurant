using AppRestaurante.Controllers.Base;
using AppRestaurante.ViewModel;
using Services.Repositories;
using Services.ServiceException;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace AppRestaurante.Controllers
{
    public class PaymentController : BaseController<PaymentTypeRepository>
    {
        #region "Constantes"

        #region "Tabs"
        private const string TAB_CADASTRO = "cadastro-tab";
        private const string TAB_EXCLUIR  = "excluir-tab";
        #endregion

        #endregion

        #region "Index View"
        /// <summary>
        /// Return View Index
        /// </summary>
        /// <param name="tab">tab to render</param>
        /// <param name="message">message to show</param>
        /// <returns>view</returns>
        [HttpGet]        
        public async System.Threading.Tasks.Task<ActionResult> Index(string tab = TAB_CADASTRO, string message = null)
        {
            System.Collections.Generic.IEnumerable<SelectListItem> objSelectListItem;

            try
            {
                var result = await ObjRepository.GetAllPaymentTypeAsync();
                
                objSelectListItem = (from item in result.Tables[Domain.Models.PaymentTypeDomain.TABLE_NAME].AsEnumerable().ToList()
                                     select new SelectListItem()
                                     {
                                         Text     = item[Domain.Models.PaymentTypeDomain.COLUMN_PAYMENTE_NAME].ToString(),
                                         Value    = item[Domain.Models.PaymentTypeDomain.COLUMN_PAYMENTE_ID].ToString(),
                                         Selected = true
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

        #region "Insert new payment"
        /// <summary>
        /// Insert the new payment
        /// </summary>
        /// <param name="model">model from view</param>
        /// <returns>view with details from operation</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> InsertNewPayment(PaymentTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                System.Collections.Generic.List<string> errors = GetErrorsModelState();
                return RedirectToAction(nameof(ErrorView), new { Message = string.Join(",", errors) });
            }

            System.Data.DataTable dataTable = Domain.Models.PaymentTypeDomain.CreateDataTable();
            System.Data.DataRow row         = Domain.Models.PaymentTypeDomain.CreateDataRow(dataTable, model.PaymentTypeId, model.PaymentTypeName);

            dataTable.Rows.Add(row);

            System.Data.DataSet dataSet = new System.Data.DataSet();
            dataSet.Tables.Add(dataTable);
                        
            try
            {
                await ObjRepository.InsertNewPaymentAsync(dataSet);
            }
            catch(IntegrityException ex)
            {
                return RedirectToAction(nameof(ErrorView), new { Message = ex.Message });
            }

            return RedirectToAction(nameof(Index), new { tab = TAB_CADASTRO, message = Util.MessageApp.Msg_Operacao_Realizada_Com_Sucesso });
        }
        #endregion

        #region "Delete payment"
        /// <summary>
        /// Service to Delete payment
        /// </summary>
        /// <param name="model">model from view</param>
        /// <returns>view with details</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeletePayment(PaymentTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                System.Collections.Generic.List<string> errors = GetErrorsModelState();
                return RedirectToAction(nameof(ErrorView), new { Message = string.Join(",", errors) });
            }

            try 
            {
                await ObjRepository.DeletePaymentAsync(model.PaymentTypeId);
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