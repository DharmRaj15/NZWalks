using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum 3 charectors")]
        [MaxLength(3, ErrorMessage = "Code has to be maximun 3 charectors")]
        public string Code { get; set; }
        [MaxLength(100, ErrorMessage = "Name has to be maximun 100 charectors")]
        [Required]
        public string Name { get; set; }
        public string? RegionImagerl { get; set; }
    }
}
