using Newtonsoft.Json;
using Resources.Interfaces;
using Resources.Models;

namespace Resources.Services;

public class CategoryService : ICategoryService<Category, ICategory>
{
    private readonly FileService _fileService;
    private List<Category> _categories;

    public CategoryService(string filePath)
    {
        _fileService = new FileService(filePath);
        _categories = [];
        GetAllCategories();
    }

    public Response<ICategory> CreateAndAddCategoryToList(Category categoryRequest)
    {
        try
        {
            //Glöm inte alla andra checkar innan man addar till listan och sparar till fil. 
            _categories.Add(categoryRequest);

            var categoriesAsString = JsonConvert.SerializeObject(_categories);
            var response = _fileService.SaveToFile(categoriesAsString);

            if (response.Succeeded)
            {
                return new Response<ICategory> { Succeeded = true, Message = "Category was successfully created! :) " };
            }
            else
            {
                return new Response<ICategory> { Succeeded = false, Message = response.Message };
            }
        }
        catch (Exception ex)
        {
            return new Response<ICategory> { Succeeded = false, Message = ex.Message };
        }
    }

    #region Get
    public Response<IEnumerable<ICategory>> GetAllCategories()
    {
        try
        {
            var result = _fileService.GetFromFile();

            if (result.Succeeded)
            {
                _categories = JsonConvert.DeserializeObject<List<Category>>(result.Content!)!;
                return new Response<IEnumerable<ICategory>> { Succeeded = true, Content = _categories };
            }
            else
            {
                return new Response<IEnumerable<ICategory>> { Succeeded = false, Message = result.Message };
            }
        }
        catch (Exception ex)
        {
            return new Response<IEnumerable<ICategory>> { Succeeded = false, Message = ex.Message };
        }
    }

    public Response<ICategory> GetOneCategoryById(string id)
    {
        try
        {
            var category = _categories.FirstOrDefault((x) => x.Id == id);
            if (category != null)
            {
                return new Response<ICategory> { Succeeded = false, Content = category };
            }

            return new Response<ICategory> { Succeeded = false, Message = "Category could not be found." };
        }
        catch (Exception ex)
        {
            return new Response<ICategory> { Succeeded = false, Message = ex.Message };

        }
    }

    #endregion
    public Response<ICategory> UpdateCategoryById(string id, Category updatedCategory)
    {
        try
        {
            if (string.IsNullOrEmpty(updatedCategory.Name) || string.IsNullOrEmpty(updatedCategory.Id))
            {
                return new Response<ICategory> { Succeeded = false, Message = $"Could not update because all required fields was not provided." };
            }

            //om det namnet är unikt eller om namnet ej är unikt då det räknar med sigsjälv (dvs gamla namnet behålls på kategorin)
            var existingCategory = _categories.FirstOrDefault(x => x.Name.ToLower() == updatedCategory.Name.ToLower() && x.Id != id);
            if (existingCategory != null)
            {
                return new Response<ICategory> { Succeeded = false, Message = $"Category with the name '{updatedCategory.Name}' already exists." };
            }

            var indexToUpdate = _categories.FindIndex((x) => x.Id == id);
            if (indexToUpdate > -1)
            {
                _categories[indexToUpdate] = updatedCategory;

                var updatedCategoriesAsString = JsonConvert.SerializeObject(_categories);
                var response = _fileService.SaveToFile(updatedCategoriesAsString);

                if (response.Succeeded)
                {
                    return new Response<ICategory> { Succeeded = true, Message = "Category was successfully updated!" };
                }
                else
                {
                    return new Response<ICategory> { Succeeded = false, Message = "Something went wrong when saving the updated category to file." };
                }
            }
            else
            {
                return new Response<ICategory> { Succeeded = false, Message = $"Category could not be found and could not be updated." };
            }
           
        }
        catch (Exception ex)
        {
            return new Response<ICategory> { Succeeded = false, Message = $"Something went wrong. Category was not updated: {ex.Message}" };
        }
    }


    public Response<ICategory> DeleteCategoryById(string id)
    {

        try
        {
            if (_categories.Any((x) => x.Id == id))
            {
                if (_categories.Remove(_categories.FirstOrDefault((x) => x.Id == id)!))
                {
                    var updatedCategoriesAsString = JsonConvert.SerializeObject(_categories);
                    var response = _fileService.SaveToFile(updatedCategoriesAsString);
                    if (response.Succeeded)
                    {
                        return new Response<ICategory> { Succeeded = true, Message = "Category was successfully deleted!" };
                    }

                }
                else
                {
                    return new Response<ICategory> { Succeeded = false, Message = "Something went wrong. Could not delete category." };

                }
            }
            return new Response<ICategory> { Succeeded = false, Message = "Could not find category to remove." };
        }
        catch (Exception ex)
        {
            return new Response<ICategory> { Succeeded = false, Message = ex.Message };
        }
    }
}