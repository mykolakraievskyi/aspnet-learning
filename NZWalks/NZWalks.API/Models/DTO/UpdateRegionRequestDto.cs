using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [Length(3, 3, ErrorMessage = "The code has to be exactly 3 characters long")]
        public string Code { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "The name has to be a maximum of 255 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
