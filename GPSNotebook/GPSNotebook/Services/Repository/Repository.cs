﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GPSNotebook.Models;
using SQLite;

namespace GPSNotebook.Services.Repository
{
    public class Repository : IRepository
    {

        private readonly SQLiteAsyncConnection _database;

        public Repository()
        {
            _database = new SQLiteAsyncConnection(
                Path.Combine(Constants.DatabasePath, Constants.DATABASE_NAME));
        }


        public Task CreateTableAsync<T>() where T : IEntity, new()
        {
            return _database.CreateTableAsync<T>();
        }

        public Task<List<T>> GetItemsAsync<T>() where T : IEntity, new()
        {
            return _database.Table<T>().ToListAsync();
        }

        public Task<List<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new()
        {
            return _database.Table<T>().Where(predicate).ToListAsync();
        }

        public Task<T> GetItemAsync<T>(int id) where T : IEntity, new()
        {
            return _database.GetAsync<T>(id);
        }

        public Task<T> GetItemAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new()
        {
            return _database.FindAsync(predicate);
        }

        public async Task<int> InsertItemAsync<T>(T item) where T : IEntity, new()
        {
            int result;
            try
            {
                result = await _database.InsertAsync(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = -1;
            }

            return result;
        }

        public Task<int> UpdateItemAsync<T>(T item) where T : IEntity, new()
        {
            return _database.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync<T>(T item) where T : IEntity, new()
        {
            return _database.DeleteAsync<T>(item.Id);
        }
    }
}
