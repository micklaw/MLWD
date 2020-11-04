using Dotented.Interfaces;

namespace MLWD.Components
{
    public class Testimonial : DotentedContent
    {
        public string Name { get; set; }

        public string Company { get; set; }

        public string Quote { get; set; }

        public Asset Image { get; set; }
    }
}