using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TouristPlacesProject.Models
{
    public class TouristPlaceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TouristPlaceTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string TouristPlaceTypeName { get; set; }
        public List<TouristPlace> touristPlaces { get; set; }
    }
}