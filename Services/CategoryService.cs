using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository categoryRepository;

        public CategoryService()
        {
            categoryRepository = new CategoryRepository();
        }

        // Get all categories
        public List<Category> GetAllCategories()
        {
            return categoryRepository.GetAll();
        }

        // Get a category by ID
        public Category? GetCategoryById(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentException(
                    "Category ID must be greater than zero.");
            }

            return categoryRepository.GetById(categoryId);
        }

        // Add a new category
        public int AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            category.CategoryName = category.CategoryName.Trim();

            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException(
                    "Category name cannot be empty.");
            }

            if (category.CategoryName.Length > 100)
            {
                throw new ArgumentException(
                    "Category name cannot exceed 100 characters.");
            }

            if (category.Description != null)
            {
                category.Description = category.Description.Trim();

                if (category.Description.Length == 0)
                {
                    category.Description = null;
                }

                if (category.Description != null &&
                    category.Description.Length > 500)
                {
                    throw new ArgumentException(
                        "Description cannot exceed 500 characters.");
                }
            }

            return categoryRepository.Add(category);
        }

        // Update an existing category
        public bool UpdateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (category.CategoryId <= 0)
            {
                throw new ArgumentException(
                    "Category ID must be greater than zero.");
            }

            category.CategoryName = category.CategoryName.Trim();

            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException(
                    "Category name cannot be empty.");
            }

            if (category.CategoryName.Length > 100)
            {
                throw new ArgumentException(
                    "Category name cannot exceed 100 characters.");
            }

            if (category.Description != null)
            {
                category.Description = category.Description.Trim();

                if (category.Description.Length == 0)
                {
                    category.Description = null;
                }

                if (category.Description != null &&
                    category.Description.Length > 500)
                {
                    throw new ArgumentException(
                        "Description cannot exceed 500 characters.");
                }
            }

            if (categoryRepository.GetById(category.CategoryId) == null)
            {
                throw new InvalidOperationException(
                    "Category not found.");
            }

            return categoryRepository.Update(category);
        }

        // Delete a category
        public bool DeleteCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentException(
                    "Category ID must be greater than zero.");
            }

            if (categoryRepository.GetById(categoryId) == null)
            {
                throw new InvalidOperationException(
                    "Category not found.");
            }

            return categoryRepository.Delete(categoryId);
        }
    }
}