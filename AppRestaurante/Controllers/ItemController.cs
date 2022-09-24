using Services.ServiceException;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AppRestaurante.Controllers
{
    public class ItemController : Base.BaseController<Services.Repositories.ItemRepository>
    {
        #region "Constantes"
        /// <summary>
        /// insert tab
        /// </summary>
        public const string TAB_CADASTRO = "cadastro-tab";
        /// <summary>
        /// update tab
        /// </summary>
        public const string TAB_ATUALIZAR = "atualizar-tab";
        /// <summary>
        /// exclude tab
        /// </summary>
        public const string TAB_EXCLUIR = "excluir-tab";
        #endregion

        #region "Index"
        /// <summary>
        /// tela para edição dos itens
        /// </summary>
        /// <param name="tab">tab para redirecionar ao iniciar a pagina</param>
        /// <param name="message">mensagem para exibir ao carregar a pagina</param>
        /// <returns>View index</returns>
        public async System.Threading.Tasks.Task<ActionResult> Index(string tab = TAB_CADASTRO, string message = "")
        {
            System.Collections.Generic.IEnumerable<SelectListItem> objSelectListItem;

            try
            {
                System.Collections.Generic.List<System.Data.DataRow> result = await ObjRepository.GetAllItemsAsync();
            
                objSelectListItem = (from item in result
                                     select new SelectListItem()
                                     {
                                         Text     = (string) item[Domain.Models.ItemDomain.COLUMN_ITEM_NAME],
                                         Value    = item[Domain.Models.ItemDomain.COLUMN_ITEM_ID].ToString(),
                                         Selected = false
                                     }).ToList();

                objSelectListItem = objSelectListItem.Prepend(new SelectListItem { Text = "--Selecione--", Value = " ", Selected = true });
            } 
            catch(Exception ex)
            {
                return RedirectToAction(nameof(ErrorView), new { Message = ex.Message });                                                                 
            }

            ViewData["tab"]      = tab;
            ViewData["message"]  = message;
            ViewData["AllItens"] = objSelectListItem;
            ViewData["ItemCategory"] = GetItensCategory();

            return View();
        }

        private System.Collections.Generic.IEnumerable<SelectListItem> GetItensCategory()
        {
            System.Collections.Generic.List<SelectListItem> listCategory = new System.Collections.Generic.List<SelectListItem>();
            listCategory.Add(new SelectListItem { Text = "--Selecione--", Value = "", Selected=true });
            listCategory.Add(new SelectListItem { Text = "Entrada", Value = "E" });
            listCategory.Add(new SelectListItem { Text = "Prato Principal", Value = "P" });
            listCategory.Add(new SelectListItem { Text = "Sobremesa", Value = "S" });
            listCategory.Add(new SelectListItem { Text = "Bebidas", Value = "B" });
        
            return listCategory;
        }
        #endregion

        #region "Insert Itens"
        /// <summary>
        /// Insert new item
        /// </summary>
        /// <param name="item">viewModel with parameters</param>
        /// <returns>View</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> InsertNewItem(ViewModel.ItemViewModel item)
        {
            if (!ModelState.IsValid)
            {
                System.Collections.Generic.List<string> errors = GetErrorsModelState();
                return RedirectToAction(nameof(ErrorView), new { Message = string.Join(",", errors) });                                    
            }

            System.Data.DataTable dataTable = Domain.Models.ItemDomain.CreateDataTable();
            System.Data.DataRow row = Domain.Models.ItemDomain.CreateDataRow(dataTable, item.ItemId, item.ItemName, item.ItemPrice, item.ItemCategory);
            dataTable.Rows.Add(row);

            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dataTable);

            try
            {
                await ObjRepository.InsertNewItemAsync(ds);
                return RedirectToAction(nameof(Index), new { tab = TAB_CADASTRO, message = Util.MessageApp.Msg_Operacao_Realizada_Com_Sucesso });
                
            }
            catch(IntegrityException ex)
            {
                return RedirectToAction(nameof(ErrorView), new { Message = ex.Message });
            }
        }
        #endregion

        #region "Update Itens"
        /// <summary>
        /// update data from item
        /// </summary>
        /// <param name="item">viewModel with parameters</param>
        /// <returns>View</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> UpdateItem(ViewModel.ItemViewModel item)
        {
            if (!ModelState.IsValid)
            {
                System.Collections.Generic.List<string> errors = GetErrorsModelState();
                return RedirectToAction(nameof(ErrorView), new { Message = string.Join(",", errors) });
            }

            System.Data.DataTable dataTable = Domain.Models.ItemDomain.CreateDataTable();
            System.Data.DataRow row = Domain.Models.ItemDomain.CreateDataRow(dataTable, item.ItemId, item.ItemName, item.ItemPrice, item.ItemCategory);
            dataTable.Rows.Add(row);

            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dataTable);

            try
            {
                await ObjRepository.UpdateItemAsync(ds);
                return RedirectToAction(nameof(Index), new { tab = TAB_ATUALIZAR, message = Util.MessageApp.Msg_Operacao_Realizada_Com_Sucesso });
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(Index), new { tab = TAB_ATUALIZAR, message = ex.Message });
            }
        }
        #endregion

        #region "Exclude Itens"
        /// <summary>
        /// exclude item using id
        /// </summary>
        /// <param name="item">viewModel with parameters</param>
        /// <returns>View</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteItem(ViewModel.ItemViewModel item)
        {
            if (!ModelState.IsValid)
            {
                System.Collections.Generic.List<string> errors = GetErrorsModelState();
                return RedirectToAction(nameof(ErrorView), new { Message = string.Join(",", errors) });
            }
                        
            try
            {
                await ObjRepository.DeleteItemAsync(item.ItemId);
                return RedirectToAction(nameof(Index), new { tab = TAB_EXCLUIR, message = Util.MessageApp.Msg_Operacao_Realizada_Com_Sucesso });
            }
            catch (IntegrityException ex)
            {
                return RedirectToAction(nameof(Index), new { tab = TAB_EXCLUIR, message = ex.Message });
            }
        }
        #endregion

        #region "Get Item By Id"
        /// <summary>
        /// Get details item by id
        /// </summary>
        /// <param name="id">id from product to search</param>
        /// <returns></returns>
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> GetItemById(int id)
        {
            var result = await System.Threading.Tasks.Task.FromResult(ObjRepository.GetItemByIdAsync(id));
            var item = result.Result.Tables[Domain.Models.ItemDomain.TABLE_NAME].Rows[0];
            
            return Json(new { 
                               ItemId       = item[Domain.Models.ItemDomain.COLUMN_ITEM_ID], 
                               ItemName     = item[Domain.Models.ItemDomain.COLUMN_ITEM_NAME],
                               ItemPrice    = item[Domain.Models.ItemDomain.COLUMN_ITEM_PRICE] ,
                               ItemCategory = item[Domain.Models.ItemDomain.COLUMN_ITEM_CATEGORY]
                            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}