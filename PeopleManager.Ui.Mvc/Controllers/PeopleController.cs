using Microsoft.AspNetCore.Mvc;
using PeopleManager.Sdk;
using PeopleManager.Services.Model.Requests;
using PeopleManager.Services.Model.Results;
using Vives.Services.Model;

namespace PeopleManager.Ui.Mvc.Controllers
{
	public class PeopleController : Controller
	{
        private readonly PersonSdk _personSdk;

        public PeopleController(PersonSdk personSdk)
		{
            _personSdk = personSdk;
		}

		public async Task<IActionResult> Index()
		{
			var people = await _personSdk.Find();
			return View(people);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(PersonRequest person)
		{
			if (!ModelState.IsValid)
            {
                return ShowCreateView(person);
            }

			var serviceResult = await _personSdk.Create(person);
            if (!serviceResult.IsSuccess)
            {
                var errors = serviceResult
                    .Messages
                    .Where(m => m.Type == ServiceMessageType.Error)
                    .ToList();
                foreach (var error in errors)
                {
					ModelState.AddModelError("", error.Message);
                }

                return ShowCreateView(person);
            }

			return RedirectToAction("Index");
		}

        

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var person = await _personSdk.Get(id);

			if (person is null)
			{
				return RedirectToAction("Index");
			}

			return View(person);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, PersonRequest person)
		{
			if (!ModelState.IsValid)
            {
                return await ShowEditView(id, person);
            }

			var serviceResult = await _personSdk.Update(id, person);

            if (!serviceResult.IsSuccess)
            {
                var errors = serviceResult
                    .Messages
                    .Where(m => m.Type == ServiceMessageType.Error)
                    .ToList();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error.Message);
                }

                return await ShowEditView(id, person);
            }

            return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var person = await _personSdk.Get(id);

			if (person is null)
			{
				return RedirectToAction("Index");
			}

			return View(person);
		}

		[HttpPost]
		[Route("People/Delete/{id}")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var serviceResult = await _personSdk.Delete(id);

            if (!serviceResult.IsSuccess)
            {
                var errors = serviceResult
                    .Messages
                    .Where(m => m.Type == ServiceMessageType.Error)
                    .ToList();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error.Message);
                }

                return await ShowDeleteView(id);
            }

            return RedirectToAction("Index");
		}

        private IActionResult ShowCreateView(PersonRequest request)
        {
            var personResult = new PersonResult
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                Email = request.Email
            };

            return View("Create", personResult);
        }

        private async Task<IActionResult> ShowEditView(int id, PersonRequest request)
        {
            var personResult = await _personSdk.Get(id);
            if (personResult is null)
            {
                return RedirectToAction("Index");
            }
            personResult.FirstName = request.FirstName;
            personResult.LastName = request.LastName;
            personResult.Age = request.Age;
            personResult.Email = request.Email;

            return View("Edit", personResult);
        }

        private async Task<IActionResult> ShowDeleteView(int id)
        {
            var personResult = await _personSdk.Get(id);
            return View("Delete", personResult);
        }
    }
}
