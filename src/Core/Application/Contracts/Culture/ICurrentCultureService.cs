﻿namespace Application.Contracts.Culture
{
    public interface ICurrentCultureService
    {
        public string GetCurrentCulture();
        public string GetCurrentUICulture();
    }
}
