using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using TestWeb.Models.FilesLibrary;

namespace TestWeb.Models
{
    public class RenameModel
    {
        [Required]
        [Display(Name = "File")]
        public String File { get; set; }

        [Required]
        [Display(Name = "New name")]
        public string NewName { get; set; }
    }

    public class MoveModel
    {
        [Required]
        [Display(Name = "File")]
        public String File { get; set; }

        [Required]
        [Display(Name = "New path")]
        public string NewPath { get; set; }
    }

    public class CopyModel
    {
        [Required]
        [Display(Name = "File")]
        public String File { get; set; }

        [Required]
        [Display(Name = "New path")]
        public string NewPath { get; set; }
    }
}