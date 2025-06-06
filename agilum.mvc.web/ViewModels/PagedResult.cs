﻿using System;
using System.Collections.Generic;

namespace agilum.mvc.web.ViewModels
{
    public class PagedViewModel<T> : IPagedList where T : class
    {
        public string ReferenceAction { get; set; }
        public IEnumerable<T> List { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
        public int TotalResults { get; set; }
        public double TotalPages => Math.Ceiling((double)TotalResults / PageSize);
        public string ReferenceController { get; set ; }
        public string Level { get; set; }
    }
    public interface IPagedList
    {
        public string ReferenceAction { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
        public int TotalResults { get; set; }
        public double TotalPages { get; }
        public string ReferenceController { get; set; }
        public string Level { get; set; }
    }
}
