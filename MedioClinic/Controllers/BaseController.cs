using CMS.Helpers;
using Core.Configuration;
using Kentico.Content.Web.Mvc;
using MedioClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedioClinic.Controllers
{
	public class BaseController : Controller
	{
		protected readonly ILogger<BaseController> _logger;

		protected readonly IOptionsMonitor<XperienceOptions> _optionsMonitor;

		public BaseController(ILogger<BaseController> logger, IOptionsMonitor<XperienceOptions> optionsMonitor)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		}

		protected PageViewModel GetPageViewModel(
	IPageMetadata pageMetadata,
	string? message = default,
	bool displayMessage = true,
	bool displayAsRaw = default,
	MessageType messageType = MessageType.Info)
	=>
	PageViewModel.GetPageViewModel(
		pageMetadata,
		message, 
		displayMessage, 
		displayAsRaw, 
		messageType);

protected PageViewModel<TViewModel> GetPageViewModel<TViewModel>(
	IPageMetadata pageMetadata,
	TViewModel data,
	string? message = default,
	bool displayMessage = true, 
	bool displayAsRaw = default, 
	MessageType messageType = MessageType.Info)
	=>
	PageViewModel<TViewModel>.GetPageViewModel(
		data, 
		pageMetadata,
		message, 
		displayMessage, 
		displayAsRaw, 
		messageType);

		protected string Localize(string resourceKey) =>
	ResHelper.GetString(resourceKey);

		protected ActionResult InvalidInput<TUploadViewModel>(
			PageViewModel<TUploadViewModel> uploadModel)
			where TUploadViewModel : class, new()
		{
			var viewModel = GetPageViewModel(
                (IPageMetadata)uploadModel.Data,
				Localize("General.InvalidInput.Title"),
				Localize("General.InvalidInput.Message"),
				true,
				false,
				MessageType.Error);

			return View(viewModel);
		}

		protected string ConcatenateContactAdmin(string messageKey) =>
			Localize(messageKey)
				+ " "
				+ Localize("General.ContactAdministrator");
	}
}
