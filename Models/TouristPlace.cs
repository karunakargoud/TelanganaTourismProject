using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TouristPlacesProject.Models
{
    public class TouristPlace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TouristPlaceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TouristPlaceName { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string TouristPlaceDescription { get; set; }

        [Required]
        [MaxLength(200)]
        public string MoreInfo {  get; set; }

        [MaxLength(150)]
        public string ImagePath { get; set; }

        [Required]
        public int TouristPlaceTypeId { get; set; }
        public TouristPlaceType touristPlaceType { get; set; }

        [NotMapped]
        public HttpPostedFileBase Files { get; set; }
        [NotMapped]
        public string TpImageFile {  get; set; }
    }
}