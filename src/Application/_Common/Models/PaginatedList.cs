﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application._Common.Models
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items { get; }

        public int PageNumber { get; }

        public int TotalCount { get; }

        public int TotalPages { get; }

        public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public PaginatedList(IEnumerable<T> items, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(items.Count() / (double)pageSize);
            TotalCount = items.Count();
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            int count = await source.CountAsync();

            IEnumerable<T> items = await source.Skip((pageNumber - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}