using Bulky.Models;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Bulky.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Controllers
{ 
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "The DisplayOrder cann't exactly match the name.");
            }
            if(ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            return View(category );
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id==0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get(x=> x.Id==id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "The DisplayOrder cann't exactly match the name.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteById(int? id)
        { 
            var getCategory = _unitOfWork.Category.Get(x => x.Id == id);
            if (getCategory == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(getCategory);
            _unitOfWork.Save();
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
