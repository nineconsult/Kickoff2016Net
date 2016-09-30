﻿using System;
using Abp.Localization;
using Abp.Localization.Sources;

namespace Abp.Web.Localization
{
    /// <summary>
    ///     This class is used to simplify getting localized messages in this assembly.
    /// </summary>
    internal static class AbpWebLocalizedMessages
    {
        private static readonly ILocalizationSource Source;
        public const string SourceName = "AbpWeb";

        static AbpWebLocalizedMessages()
        {
            Source = LocalizationHelper.GetSource(SourceName);
        }

        public static string InternalServerError
        {
            get { return L("InternalServerError"); }
        }

        public static string ValidationError
        {
            get { return L("ValidationError"); }
        }

        public static string ValidationNarrativeTitle
        {
            get { return L("ValidationNarrativeTitle"); }
        }

        private static string L(string name)
        {
            try
            {
                return Source.GetString(name);
            }
            catch (Exception)
            {
                return name;
            }
        }
    }
}