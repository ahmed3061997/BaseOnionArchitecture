﻿using Application.Models.Common;
using Domain.Enums;

namespace Application.Models.System
{
    public class PageDto
    {
        public int? Id { get; set; }
        public int ModuleId { get; set; }
        public Pages Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
        public IEnumerable<PageOperationDto> Operations { get; set; }
    }
}
