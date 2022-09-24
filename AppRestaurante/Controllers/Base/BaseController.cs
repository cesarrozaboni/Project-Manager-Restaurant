using AppRestaurante.ViewModel;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppRestaurante.Controllers.Base
{
    public class BaseController<T> : Controller where T : class, new()
    {
        private readonly T _objRepository;

        protected T ObjRepository { get => _objRepository; }

        public BaseController()
        {
            _objRepository = new T();
        }

        #region "Error"
        /// <summary>
        /// Tela para exibir erros da aplicação
        /// </summary>
        /// <param name="message">mensagem de erro</param>
        /// <returns>View</returns>
        public ActionResult ErrorView(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Request.AnonymousID
            };

            return View("_ErrorView", viewModel);
        }
        #endregion

        public List<string> GetErrorsModelState()
        {
            List<string> errors = new List<string>();

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                    errors.Add(error.ErrorMessage);
            }

            return errors;
        }
    }
}