using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        // [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
        // [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        [Length(3, 3, ErrorMessage = "The code has to be exactly 3 characters long")]
        public string Code { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "The name has to be a maximum of 255 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
