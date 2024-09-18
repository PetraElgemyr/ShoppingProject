﻿using ShoppingApp.Models;

namespace ShoppingApp.Services;

public class CategoryService
{
    private List<Category> _categories = [];
    public Response GetAllCategories()
    {
        try
        {
            return new Response { Succeeded = true, Content = _categories };
        }
        catch (Exception ex)
        {
            return new Response { Succeeded = false, Message = $"Could not fetch categories: {ex.Message}" };
        }
    }

    public Response AddNewCategoryToList(Category categoryRequest)
    {
        try
        {
            if (string.IsNullOrEmpty(categoryRequest.Name))
            {
                return new Response { Succeeded = false, Message = $"Category name was not provided." };
            }

            if (
                !_categories.Any((x) => x.Name == categoryRequest.Name))
            {
                _categories.Add(categoryRequest);
                return new Response { Succeeded = true, Message = $"Category was successfully added to the list!" };
            }
            else
            {
                return new Response { Succeeded = false, Message = $"Category with the same name already exixts. Could not add new category to list." };
            }
        }
        catch (Exception ex)
        {
            return new Response { Succeeded = false, Message = $"Category was not added to the list: {ex.Message}" };
        }
    }

    public Response UpdateCategoryById(Category updatedCategory)
    {
        try
        {
            if (!_categories.Any((x) => x.Name == updatedCategory.Name))
            {
                return new Response { Succeeded = false, Message = $"Category name aldready exists." };
            }

            var indexToRemoveCategory = _categories.FindIndex((x) => x.CategoryId == updatedCategory.CategoryId);
          
            if (indexToRemoveCategory > -1)
            {
                _categories[indexToRemoveCategory] = updatedCategory;
                return new Response { Succeeded = true, Message = "Category was successfully updated!" };
            }
            else
            {
                return new Response { Succeeded = false, Message = $"Category could not be found and could not be updated." };
            }
        }
        catch (Exception ex)
        {
            return new Response { Succeeded = false, Message = $"Something went wrong. Category was not updated: {ex.Message}" };
        }
    }
    public Response DeleteCategoryById(string id)
    {
        try
        {
            var categoryToDelete = _categories.FirstOrDefault((x) => x.CategoryId == id);
        }
        catch (Exception ex)
        {
            return new Response { Succeeded = false, Message = $"Category was not deleted: {ex.Message}" };
        }
        //Note! Deletes all products with the choosen categoryId too
    }

}
