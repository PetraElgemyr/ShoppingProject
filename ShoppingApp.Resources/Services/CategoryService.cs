using Newtonsoft.Json;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Services;

public class CategoryService : ICategoryService
{
    private List<Category> _categories = [];
    private readonly ICategoryFileService _fileService;

    public CategoryService(ICategoryFileService fileService)
    {
        _fileService = fileService;
    }

    public RequestResponse<Category> CreateAndAddCategoryToList(Category categoryRequest)
    {
        try
        {
            var categoryWithSameName = _categories.FirstOrDefault(x => x.Name.ToLower() == categoryRequest.Name.ToLower() && x.Id != categoryRequest.Id);
            if (categoryWithSameName != null)
            {
                return new RequestResponse<Category> { Succeeded = Status.Exists, Message = $"Another category with the name '{categoryRequest.Name}' already exists." };
            }

            if (string.IsNullOrEmpty(categoryRequest.Name) || string.IsNullOrEmpty(categoryRequest.Id))
            {
                return new RequestResponse<Category> { Succeeded = Status.Failed, Message = $"Could not update because a category name was not provided." };
            }


            int amountOfCategoriesFromTheStart = _categories.Count();
            _categories.Add(categoryRequest);

            var categoriesAsString = JsonConvert.SerializeObject(_categories);
            var RequestResponse = _fileService.SaveToFile(categoriesAsString);

            if (RequestResponse.Succeeded == Status.Success && amountOfCategoriesFromTheStart + 1 == _categories.Count())
            {
                return new RequestResponse<Category> { Succeeded = Status.Success, Message = "Category was successfully created and saved to file! :)" };
            }
            else
            {
                return new RequestResponse<Category> { Succeeded = Status.SuccessWithErrors, Message = "Category was created and added to the list but the list was not saved to the file." };
            }
        }
        catch (Exception ex)
        {
            return new RequestResponse<Category> { Succeeded = Status.Failed, Message = ex.Message };
        }
    }

    #region Get
    public RequestResponse<IEnumerable<Category>> GetAllCategories()
    {
        try
        {
            var result = _fileService.GetFromFile();

            if (result.Succeeded == Status.Success && !string.IsNullOrEmpty(result.Content))
            {
                _categories = JsonConvert.DeserializeObject<List<Category>>(result.Content!)!;
                return new RequestResponse<IEnumerable<Category>> { Succeeded = Status.Success, Content = _categories };
            }
            else
            {
                return new RequestResponse<IEnumerable<Category>> { Succeeded = Status.Failed, Message = "Could not fetch categories from file." };
            }
        }
        catch (Exception ex)
        {
            return new RequestResponse<IEnumerable<Category>> { Succeeded = Status.Failed, Message = ex.Message };
        }
    }

    public RequestResponse<Category> GetOneCategoryById(string id)
    {
        try
        {
            var category = _categories.FirstOrDefault((x) => x.Id == id);
            if (category != null)
            {
                return new RequestResponse<Category> { Succeeded = Status.Success, Content = category };
            }

            return new RequestResponse<Category> { Succeeded = Status.NotFound, Message = "Category could not be found." };
        }
        catch (Exception ex)
        {
            return new RequestResponse<Category> { Succeeded = Status.Failed, Message = ex.Message };

        }
    }

    #endregion
    public RequestResponse<Category> UpdateCategoryById(string id, Category updatedCategory)
    {
        try
        {
            if (string.IsNullOrEmpty(updatedCategory.Name) || string.IsNullOrEmpty(updatedCategory.Id))
            {
                return new RequestResponse<Category> { Succeeded = Status.Failed, Message = $"Could not update because all required fields was not provided." };
            }

            //om det namnet är unikt eller om namnet ej är unikt då det räknar med sigsjälv (dvs gamla namnet behålls på kategorin)
            var existingCategory = _categories.FirstOrDefault(x => x.Name.ToLower() == updatedCategory.Name.ToLower() && x.Id != id);
            if (existingCategory != null)
            {
                return new RequestResponse<Category> { Succeeded = Status.Exists, Message = $"Category with the name '{updatedCategory.Name}' already exists." };
            }

            var indexToUpdate = _categories.FindIndex((x) => x.Id == id);
            if (indexToUpdate > -1)
            {
                _categories[indexToUpdate] = updatedCategory;

                // samma sak men lite "snyggare" kanske?? TODO: kanske ändra senare
                //_categories[indexToUpdate].Name = updatedCategory.Name;
                //_categories[indexToUpdate].Description = updatedCategory.Description;


                var updatedCategoriesAsString = JsonConvert.SerializeObject(_categories);
                var RequestResponse = _fileService.SaveToFile(updatedCategoriesAsString);

                if (RequestResponse.Succeeded == Status.Success)
                {
                    return new RequestResponse<Category> { Succeeded = Status.Success, Message = "Category was successfully updated and saved to the file!", Content = updatedCategory };
                }
                else
                {
                    return new RequestResponse<Category> { Succeeded = Status.SuccessWithErrors, Message = "Category was updated but something went wrong when saving the list to file." };
                }
            }
            else
            {
                return new RequestResponse<Category> { Succeeded = Status.NotFound, Message = $"Category could not be found and could not be updated." };
            }

        }
        catch (Exception ex)
        {
            return new RequestResponse<Category> { Succeeded = Status.Failed, Message = $"Something went wrong. Category was not updated: {ex.Message}" };
        }
    }


    public RequestResponse<Category> DeleteCategoryById(string id)
    {

        try
        {
            if (_categories.Any((x) => x.Id == id))
            {
                if (_categories.Remove(_categories.FirstOrDefault((x) => x.Id == id)!))
                {
                    var updatedCategoriesAsString = JsonConvert.SerializeObject(_categories);
                    var RequestResponse = _fileService.SaveToFile(updatedCategoriesAsString);
                    if (RequestResponse.Succeeded == Status.Success)
                    {
                        return new RequestResponse<Category> { Succeeded = Status.Success, Message = "Category was successfully deleted and list was saved to file!" };
                    }
                }
                else
                {
                    return new RequestResponse<Category> { Succeeded = Status.Failed, Message = "Something went wrong. Could not delete category." };

                }
            }
            return new RequestResponse<Category> { Succeeded = Status.NotFound, Message = "Could not find category to remove." };
        }
        catch (Exception ex)
        {
            return new RequestResponse<Category> { Succeeded = Status.Failed, Message = ex.Message };
        }
    }
}
