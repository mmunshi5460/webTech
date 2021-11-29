using Microsoft.AspNetCore.Mvc;
using MMClubsClasses.Utilities;
using MMClubsClasses.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace MMClubs.Models
{
    [ModelMetadataTypeAttribute(typeof(MMNameAddressMetadata))]
    public partial class NameAddress : IValidatableObject
    {
        private readonly MMClubsContext _context = new MMClubsContext();
        /// <summary>
        /// using firstname, last name to get the full name
        /// </summary>
        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                    return ($"{LastName},{FirstName}");
                else
                {
                    if (string.IsNullOrEmpty(FirstName))
                        return $"{LastName}";
                    else
                        if (string.IsNullOrEmpty(LastName))
                        return $"{FirstName}";
                }
                return "Name not found";
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string emptyString = "";
            if (string.IsNullOrEmpty(FirstName)
                && string.IsNullOrEmpty(LastName)
                && string.IsNullOrEmpty(CompanyName))
            {
                yield return new ValidationResult("atleast one of first name, last name or company name must be provided", new[] { nameof(FirstName), nameof(LastName), nameof(CompanyName) });
            }
            else
            {
                if (string.IsNullOrEmpty(FirstName))
                    FirstName = emptyString;
                else
                {
                    FirstName = MMStringManipulation.MMCapitalize(FirstName.Trim());
                }
                if (string.IsNullOrEmpty(LastName))
                    LastName = emptyString;
                else
                {
                    LastName = MMStringManipulation.MMCapitalize(LastName.Trim());
                }
                if (string.IsNullOrEmpty(CompanyName))
                    CompanyName = emptyString;
                else
                {
                    CompanyName = MMStringManipulation.MMCapitalize(CompanyName.Trim());
                }
            }
            if (string.IsNullOrEmpty(StreetAddress))
                StreetAddress = emptyString;
            else
            {
                StreetAddress = MMStringManipulation.MMCapitalize(StreetAddress.Trim());
            }
            if (string.IsNullOrEmpty(City))
                City = emptyString;
            else
            {
                City = MMStringManipulation.MMCapitalize(City.Trim());
            }
            if (string.IsNullOrEmpty(Phone))
                Phone = emptyString;
            else
            {
                Phone = MMStringManipulation.MMExtractDigits(Phone.Trim());
            }
            
            if (!string.IsNullOrEmpty(ProvinceCode))
            {
                var provinceList = _context.Province.Where(p => p.ProvinceCode.Equals(ProvinceCode));
                if (!provinceList.Any())
                {
                    yield return new ValidationResult("Province not found",
                        new[] { nameof(ProvinceCode) });
                }
            }
            if (string.IsNullOrEmpty(PostalCode))
                PostalCode = emptyString;
            else
            {
                PostalCode = PostalCode.Trim();
                if (string.IsNullOrEmpty(ProvinceCode))
                {
                    yield return new ValidationResult($"Province is required if postal code is entered", new[] { nameof(ProvinceCode) });
                }
                else
                {


                    var provinceList = _context.Province.Where(p => p.ProvinceCode.Equals(ProvinceCode));
                    var province = _context.Province.Find(ProvinceCode);

                    var country = _context.Country.FirstOrDefault(c => c.CountryCode.Equals(province.CountryCode));

                    if (!provinceList.Any())
                    {
                        yield return new ValidationResult($"Either the province is empty or invalid", new[] { nameof(ProvinceCode) });
                    }

                    else
                    {
                        var postal = country.PostalPattern;
                        var check = MMStringManipulation.MMPostalCodeIsValid(PostalCode, postal);
                        if (!check)
                            yield return new ValidationResult($"Postal code pattern is invalid", new[] { (PostalCode) });
                    }
                    PostalCode = PostalCode.ToUpper();
                    if (country.CountryCode.Equals("CA") && !string.IsNullOrEmpty(PostalCode))
                    {
                        var firstLetter = PostalCode.Substring(0, 1);
                        if (province.FirstPostalLetter.Contains(firstLetter))
                        {
                            PostalCode.Insert(3, " ");
                        }
                        else
                        {
                            yield return new ValidationResult($"First Letter of Postal Code does not match the Province", new[] { nameof(ProvinceCode) });
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(Phone))
                Phone = emptyString;
            else
            {
                Phone = Phone.Trim();
                Phone = MMStringManipulation.MMExtractDigits(Phone);
                if (Phone.Length != 10)
                {
                    yield return new ValidationResult($"phone must be exactly 10 digits", new[] { (Phone) });     
                }
                else
                {
                    string phoneFormat = "###-###-####";
                    Phone = System.Convert.ToInt64(Phone).ToString(phoneFormat);
                }
            }
            if (string.IsNullOrEmpty(Email) && (string.IsNullOrEmpty(PostalCode) && string.IsNullOrEmpty(StreetAddress) && string.IsNullOrEmpty(City) && string.IsNullOrEmpty(ProvinceCode)))
            {
                yield return new ValidationResult($"all the postal addressing information is required, if email is not provided", new[] { (PostalCode) });
            }

            yield return ValidationResult.Success;
        }
    }
    public class MMNameAddressMetadata
    {
        [Display(Name = "ID")]
        public int NameAddressId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Province Code")]
        public string ProvinceCode { get; set; }
        [MMEmailAnnotationAttribute()]
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
