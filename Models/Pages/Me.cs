using Dotented;
using Dotented.Interfaces;
using MLWD.Components;

namespace MLWD.Pages
{
    public class Me : DotentedContent
    {
        public static string Query = @"
            query {
                pages: meCollection(limit: 1) {
                    items {
                        __typename
                        aboutTitle
                        aboutLead
                        aboutBody
                        bioTitle
                        bioBody
                        codedLead
                        codedTitle
                        companyName
                        contactLead
                        contactTitle
                        email
                        githubUrl
                        linkedinUrl
                        name
                        url
                        skillsTitle
                        testimonialTitle
                        toolsLead
                        toolsTitle
                        twitterUrl
                        startYear
                        metaTitle
                        metaDescription
                        tools
                        companies: codedCompaniesCollection {
                            items {
                                __typename
                                title
                                url
                            }
                        }
                        profileImage {
                            __typename
                            title
                            url
                        }
                        testimonials: testimonialsCollection {
                            items {
                                __typename
                                name
                                company
                                quote
                                image {
                                    __typename
                                    title
                                    url
                                }
                            }
                        }
                        skills: skillsCollection {
                            items {
                                __typename
                                title
                                icon
                                details
                            }
                        }
                    }
                }
            }
        ";

        public string AboutTitle { get; set; }

        public string AboutLead { get; set; }

        public string AboutBody { get; set; }

        public string BioTitle { get; set; }

        public string BioBody { get; set; }

        public string CodedLead { get; set; }

        public string CodedTitle { get; set; }

        public string CompanyName { get; set; }

        public string ContactLead { get; set; }

        public string ContactTitle { get; set; }

        public string Email { get; set; }

        public string GithubUrl { get; set; }

        public string LinkedinUrl { get; set; }

        public string Name { get; set; }

        public string SkillsTitle { get; set; }

        public string TestimonialTitle { get; set; }

        public string ToolsLead{ get; set; }

        public string ToolsTitle { get; set; }

        public string TwitterUrl { get; set; }

        public string StartYear { get; set; }

        public string MetaTitle { get; set; }

        public string MetaDescription { get; set; }

        public string[] Tools { get; set; }

        public Components.Asset ProfileImage { get; set; }

        public DotentedCollection<Components.Asset> Companies { get; set; }

        public DotentedCollection<Testimonial> Testimonials { get; set; }

        public DotentedCollection<Skills> Skills { get; set; }
    }
}