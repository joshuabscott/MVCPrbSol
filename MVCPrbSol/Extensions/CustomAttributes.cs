using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Extensions
{
    //public class CustomAttributes
    //{
    //}

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            //inheritance happening when you see override, extend further then we could normally reach. method name is valid, bass in the object, the value of the property, validationContect is used, passed. by decorating or data
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { _maxFileSize} bytes.";
        }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute    
    {
        private readonly string[] _extensions;        
        public AllowedExtensionsAttribute(string[] extensions)        
        {           
            _extensions = extensions;        
        }        
        protected override ValidationResult IsValid(        
            object value, ValidationContext validationContext)        
        {            
            var file = value as IFormFile;            
            if (file != null)            
            {                
                var extension = Path.GetExtension(file.FileName);                
                if (!_extensions.Contains(extension.ToLower()))                
                {                    
                    return new ValidationResult(GetErrorMessage(extension));                
                }            
            }           
            return ValidationResult.Success;        
        }        
        public string GetErrorMessage(string ext)        
        {            
            return $"The file extension {ext} is not allowed!";        
        }    
    }
}

//overloading is two methods with the same name but different signature
//we are extending this to do new things with classes
//using maxFileSize;