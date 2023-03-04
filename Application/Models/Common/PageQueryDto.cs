﻿using Application.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Common
{
    public class PageQueryDto
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string? SortColumn { get; set; }
        public SortDirection SortDirection { get; set; }
        public string? SeachColumn { get; set; }
        public string? SeachTerm { get; set; }
    }
}