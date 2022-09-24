using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace AppRestaurante.Controllers
{
    public class HomeController : Base.BaseController<Services.Repositories.OrderRepository>
    {
        #region "Constantes"
        Services.Repositories.PaymentTypeRepository _paymentTypeRepository;
        Services.Repositories.ItemRepository _itemRepository;
        Services.Repositories.TableRepository _tableRepository;
        #endregion

        #region "Propriedades"
        public Services.Repositories.PaymentTypeRepository PaymentTypeRepository
        {
            get
            {
                return _paymentTypeRepository ?? (_paymentTypeRepository = new Services.Repositories.PaymentTypeRepository());
            }
        }

        public Services.Repositories.ItemRepository ItemRepository
        {
            get
            {
                return _itemRepository ?? (_itemRepository = new Services.Repositories.ItemRepository());
            }
        }

        public Services.Repositories.TableRepository TableRepository
        {
            get
            {
                return _tableRepository ?? (_tableRepository = new Services.Repositories.TableRepository());
            }
        }
        #endregion

        #region "Enum"
        public enum enumTableStatus
        {
            Open,
            Reserved,
            attendance,
            finished
        }
        #endregion

        #region "Constructor"
        /// <summary>
        /// constructor Home controller
        /// </summary>
        public HomeController()
        {
        }
        #endregion

        #region "Index"
        // GET: Home
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            try
            {
                var objMultipleModels = new Tuple<System.Collections.Generic.IEnumerable<SelectListItem>, System.Collections.Generic.IEnumerable<SelectListItem>, System.Collections.Generic.IEnumerable<SelectListItem>>
                    (await GetAllTableAsync(), GetAllItemsAsync(), await GetAllPaymentAsync());

                return View(objMultipleModels);
            }catch (Services.ServiceException.IntegrityException ex)
            {
                return RedirectToAction(nameof(ErrorView), ex.Message);
            }
        }
        #endregion

        #region "Get all table async"
        /// <summary>
        /// Get all table from restaurant
        /// </summary>
        /// <returns>Select List item with value</returns>
        public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SelectListItem>> GetAllTableAsync()
        {            
            System.Data.DataSet result = await TableRepository.GetAllTableAsync();

            System.Collections.Generic.IEnumerable<SelectListItem> objSelectListItem =  (from item in result.Tables[Domain.Models.TableDomain.TABLE_NAME].AsEnumerable().ToList()
                                                                 select new SelectListItem()
                                                                 {
                                                                     Text = (string)item[Domain.Models.TableDomain.COLUMN_TABLE_DESCRIPTION],
                                                                     Value = Convert.ToString(item[Domain.Models.TableDomain.COLUMN_TABLE_ID]),
                                                                     Selected = true
                                                                 }).ToList();

            objSelectListItem = objSelectListItem.Prepend(new SelectListItem { Text = "--Selecione--", Value = "", Selected = true });
            return objSelectListItem;
        }
        #endregion

        #region "Get All type Payment"
        /// <summary>
        /// Get all type of payment
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SelectListItem>> GetAllPaymentAsync()
        {
            var result = await PaymentTypeRepository.GetAllPaymentTypeAsync();

            System.Collections.Generic.IEnumerable<SelectListItem> objSelectListItem =
                (from obj in result.Tables[0].AsEnumerable()
                 select new SelectListItem {
                     Value = obj[Domain.Models.PaymentTypeDomain.COLUMN_PAYMENTE_ID].ToString(),
                     Text = (string)obj[Domain.Models.PaymentTypeDomain.COLUMN_PAYMENTE_NAME],
                     Selected = true
                 }).ToList();

            objSelectListItem = objSelectListItem.Prepend(new SelectListItem { Text = "--Selecione--", Value = "", Selected = true });
            return objSelectListItem;
        }
        #endregion

        #region "Get All itens"
        /// <summary>
        /// Get all itens async
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<SelectListItem> GetAllItemsAsync()
        {
            var result = ItemRepository.GetAllItemsAsync().GetAwaiter().GetResult();

            result = result.Where(x => x[Domain.Models.ItemDomain.COLUMN_ITEM_CATEGORY].ToString() == "E").ToList();
            System.Collections.Generic.IEnumerable<SelectListItem> objSelectListItem = (from obj in result
                                                             select new SelectListItem()
                                                             {
                                                                 Text = obj.ItemArray[1].ToString(),
                                                                 Value = obj.ItemArray[0].ToString(),
                                                                 Selected = false
                                                             }).ToList();

            objSelectListItem = objSelectListItem.Prepend(new SelectListItem { Text = "--Selecione--", Value = "", Selected = true });
            return objSelectListItem;
        }
        #endregion

        #region "Get order details"
        /// <summary>
        /// Get order details
        /// </summary>
        /// <param name="table">table of order</param>
        /// <returns></returns>
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> GetOrderDetails(int table)
        {
            try
            {
                var result = await ObjRepository.GetOrderByTableAsync(table);

                System.Collections.Hashtable hsOrder = new System.Collections.Hashtable();
                if (result != null && result.Tables[0].Rows.Count > 0)
                {
                    hsOrder["Responsavel"] = result.Tables[Domain.Models.OrderDomain.TABLE_NAME].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_CUSTOMER];
                    hsOrder["Total"]       = result.Tables[Domain.Models.OrderDomain.TABLE_NAME].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_FINAL_TOTAL];
                    hsOrder["Status"]      = result.Tables[Domain.Models.OrderDomain.TABLE_NAME].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_STATUS];
                    hsOrder["Id"]          = result.Tables[Domain.Models.OrderDomain.TABLE_NAME].Rows[0][Domain.Models.OrderDomain.COLUMN_ORDERS_ID];
                }

                return Json(new { type = "Success", result = hsOrder }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { type = "error", message = "Erro ao realizar operação, " + ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Create Reserve"
        /// <summary>
        /// Create new reserve in table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="responsable"></param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> CreateReserve(int table, string responsable)
        {
            System.Data.DataTable dt = Domain.Models.OrderDomain.CreateDataTable();
            System.Data.DataRow row = Domain.Models.OrderDomain.CreateDataRow(dt, 0, null, table, null, DateTime.Now, null, responsable, (int)enumTableStatus.Reserved);
            dt.Rows.Add(row);

            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dt);
            try
            {            
                await ObjRepository.CreateReserveAsync(ds);
                return Json(new { type = "success", data = "Ocorre um erro ao fazer a reserva!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { type = "error", data= "Ocorre um erro ao cancelar reserva, " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Cancel reservation"
        /// <summary>
        /// cancel reservation
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> DeleteReserve(int table)
        {
            try
            {
                await ObjRepository.DeleteReserveAsync(table);
                return Json(new { type = "Success", message = "Reserva cancelada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { type = "error", message = "Erro ao processsar operação" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Get item unit price"
        /// <summary>
        /// Get item unit price
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> GetItemUnitPrice(int itemId)
        {
            try
            {
                var result = await ItemRepository.GetItemUnitPriceAsync(itemId);
                return Json(new { type = "success", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { type="error", data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Get item by category"
        /// <summary>
        /// Return itens from select category
        /// </summary>
        /// <param name="category">category to search</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetItemByCategory(string category)
        {
            try
            {
                var result = ItemRepository.GetAllItemsAsync().GetAwaiter().GetResult();

                result = result.Where(x => x[Domain.Models.ItemDomain.COLUMN_ITEM_CATEGORY].ToString() == category).ToList();

                System.Collections.Generic.IEnumerable<SelectListItem> objSelectListItem =  (from obj in result
                                                                 select new SelectListItem()
                                                                 {
                                                                     Text     = obj.ItemArray[1].ToString(),
                                                                     Value    = obj.ItemArray[0].ToString(),
                                                                     Selected = false
                                                                 }).ToList();

                objSelectListItem = objSelectListItem.Prepend(new SelectListItem { Text = "--Selecione--", Value = "", Selected = true });

                return Json(objSelectListItem, JsonRequestBehavior.AllowGet);
            }
            catch(Services.ServiceException.IntegrityException ex)
            {
                return Json(new { type = "error", message = "erro ao executar operação, " + ex.Message });
            }
        }
        #endregion

        #region "Add Item in order"
        /// <summary>
        /// Add new item in order, and create order if null"
        /// </summary>
        /// <param name="orderDetail">object with values</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> AddItemInOrder(ViewModel.OrderDetailViewModel orderDetail)
        {
            try
            {

                if (orderDetail == null)
                    throw new ArgumentException("Favor informar o item que sera adicionado");

                int order;
                if (orderDetail.OrderId == 0)
                    order = AddOrder(new ViewModel.OrderViewModel { CustomerName = orderDetail.CustomerName, TableNumber = orderDetail.TableNumber });
                else
                    order = orderDetail.OrderId;


                System.Data.DataTable dt = Domain.Models.OrderDetailDomain.CreateDataTable();
                var row = Domain.Models.OrderDetailDomain.CreateDataRow(dt, null, order, orderDetail.ItemId, Convert.ToDecimal(orderDetail.UnitPrice),
                                                                        Convert.ToDecimal(orderDetail.Quantity), Convert.ToDecimal(orderDetail.Discount),
                                                                        Convert.ToDecimal(orderDetail.Total));
                dt.Rows.Add(row);

                System.Data.DataSet ds = new System.Data.DataSet();
                ds.Tables.Add(dt);

                Services.Repositories.OrderDetailRepository objOrderDetail = new Services.Repositories.OrderDetailRepository();
                await objOrderDetail.InsertItemInOrderAsync(ds);

                return Json(new { data = "Success", message = "Operação realizada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { data = "error", message = "Erro ao realizar operação, " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Add order"
        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="objOrderViewModel"></param>
        /// <returns></returns>
        public int AddOrder(ViewModel.OrderViewModel objOrderViewModel)
        {
            Services.Repositories.OrderRepository order = new Services.Repositories.OrderRepository();

            System.Data.DataTable dt = Domain.Models.OrderDomain.CreateDataTable();
            var row = Domain.Models.OrderDomain.CreateDataRow(dt, objOrderViewModel.OrderId, objOrderViewModel.PaymentTypeId, objOrderViewModel.TableNumber, objOrderViewModel.OrderNumber, objOrderViewModel.OrderDate, objOrderViewModel.FinalTotal, objOrderViewModel.CustomerName, 2);
            dt.Rows.Add(row);
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dt);

            var result = order.AddOrderAsync(ds).Result;

            return result;
        }
        #endregion

        #region "Get itens of order"
        /// <summary>
        /// Get itens of order
        /// </summary>
        /// <param name="orderId">number of order</param>
        /// <returns></returns>
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> GetItensOfOrder(int orderId)
        {
            try
            {            
                Services.Repositories.OrderDetailRepository order = new Services.Repositories.OrderDetailRepository();
                var result = await order.GetItensOfOrderAsync(orderId);

                System.Collections.Generic.List<System.Collections.Hashtable> listhsOrder = new System.Collections.Generic.List<System.Collections.Hashtable>();
                if (result != null && result.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                    {
                        System.Collections.Hashtable hsOrder = new System.Collections.Hashtable();

                        hsOrder["OrderDetailId"] = result.Tables[Domain.Models.OrderDetailDomain.TABLE_NAME].Rows[i][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_ID];
                        hsOrder["ItemId"]        = result.Tables[Domain.Models.OrderDetailDomain.TABLE_NAME].Rows[i][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_ITEM_ID];
                        hsOrder["UnitPrice"]     = result.Tables[Domain.Models.OrderDetailDomain.TABLE_NAME].Rows[i][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_UNIT_PRICE];
                        hsOrder["Quantity"]      = result.Tables[Domain.Models.OrderDetailDomain.TABLE_NAME].Rows[i][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_QUANTITY];
                        hsOrder["Discount"]      = result.Tables[Domain.Models.OrderDetailDomain.TABLE_NAME].Rows[i][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_DISCOUNT];
                        hsOrder["Total"]         = result.Tables[Domain.Models.OrderDetailDomain.TABLE_NAME].Rows[i][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_TOTAL];
                        hsOrder["ItemName"]      = result.Tables[Domain.Models.OrderDetailDomain.TABLE_NAME].Rows[i][Domain.Models.OrderDetailDomain.COLUMN_ORDER_DETAIL_ITEM_NAME];
                        listhsOrder.Add(hsOrder);
                    }
                }

                return Json(new { data = "Success", result = listhsOrder }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { data = "error", message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Remove item from order"
        /// <summary>
        /// Remove item from order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDetailId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async System.Threading.Tasks.Task<JsonResult> RemoveItemFromOrder(string orderId, string orderDetailId)
        {
            try
            {

                Services.Repositories.OrderDetailRepository order = new Services.Repositories.OrderDetailRepository();
                await order.RemoveItemFromOrderAsync(Convert.ToInt32(orderId), Convert.ToInt32(orderDetailId));
                return Json(new { type = "success", message = "Operação concluida com sucesso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { type = "error", message = "Erro ao executar operação, " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region "Final Payment"
        /// <summary>
        /// Final Payment
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> FinalPayment(string orderId, string paymentTypeId, string payment)
        {
            try
            {
                await ObjRepository.FinalPaymentAsync(Convert.ToInt32(orderId), Convert.ToInt32(paymentTypeId), Convert.ToDecimal(payment));
                return Json(new { type = "Success", message = "Pagamento realizado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Services.ServiceException.IntegrityException ex)
            {
                return Json(new { type = "error", message = "Erro ao processsar operação" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


    }
}