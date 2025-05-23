﻿using HardwareStore.Models;

namespace HardwareStore.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task EditCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        Task<Category?> GetByIdCategoryAsync(int id);

        Task<bool> CategoryNameExistsAsync(string categoryName);
    }
}
