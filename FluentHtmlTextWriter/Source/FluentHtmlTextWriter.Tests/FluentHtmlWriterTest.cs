using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.UI;
using FluentHtmlTextWriter;
using System.IO;

namespace FluentHtmlTextWriter.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FluentHtmlWriterTest
    {
        public const int PERFORMANCE_TEST_RUNS = 20000;

        public FluentHtmlWriterTest() { }

        [TestMethod]
        public void WriteSpan()
        {
            string html = FluentHtmlTextWriter.Begin()
                            .WriteTag(HtmlTextWriterTag.Span)
                            .WithAttribute(HtmlTextWriterAttribute.Id, "id_span")
                            .WithValue("contents_span")
                            .End();
            Assert.AreEqual("<span id=\"id_span\">contents_span</span>", html);
        }

        [TestMethod]
        public void WriteImg()
        {
            string html = FluentHtmlTextWriter.Begin()
                            .WriteTag(HtmlTextWriterTag.Img)
                            .WithAttribute(HtmlTextWriterAttribute.Src, @"\images\logo.png")
                            .WithAttribute("alt", "Company logo")
                            .End();
            Assert.AreEqual("<img src=\"\\images\\logo.png\" alt=\"Company logo\" />", html);
        }

        [TestMethod]
        public void WriteImg_WithStyle()
        {
            string html = FluentHtmlTextWriter.Begin()
                            .WriteTag(HtmlTextWriterTag.Img)
                            .WithAttribute(HtmlTextWriterAttribute.Src, @"\images\logo.png")
                            .WithAttribute("alt", "Company logo")
                            .WithStyle(HtmlTextWriterStyle.BorderWidth, "0px")
                            .End();
            Assert.AreEqual("<img src=\"\\images\\logo.png\" alt=\"Company logo\" style=\"border-width:0px;\" />", html);
        }

        [TestMethod]
        public void WriteImg_WithTextWriter()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            FluentHtmlTextWriter.Begin(writer)
                                .WriteTag(HtmlTextWriterTag.Img)
                                .WithAttribute(HtmlTextWriterAttribute.Src, @"\images\logo.png")
                                .WithAttribute(HtmlTextWriterAttribute.Alt, "Company logo")
                                .WithStyle(HtmlTextWriterStyle.BorderWidth, "0px")
                                .Flush();
            Assert.AreEqual("<img src=\"\\images\\logo.png\" alt=\"Company logo\" style=\"border-width:0px;\" />", builder.ToString());
        }

        [TestMethod]
        public void WriteMenu()
        {
            FluentHtmlTextWriter writer = FluentHtmlTextWriter.Begin();

            writer.BeginTag(HtmlTextWriterTag.Ul)
                  .WithAttribute(HtmlTextWriterAttribute.Id, "menu.Name")
                  .WithAttribute(HtmlTextWriterAttribute.Class, "sf-menu sf-vertical")
                  .BeginTag(HtmlTextWriterTag.Li)
                  .BeginTag(HtmlTextWriterTag.A)
                  .WithAttribute(HtmlTextWriterAttribute.Class, "sf-with-ul")
                  .WithAttribute(HtmlTextWriterAttribute.Href, "#")
                  .WithValue("Link 1")
                  .WriteTag(HtmlTextWriterTag.Span)
                  .WithAttribute(HtmlTextWriterAttribute.Class, "sf-sub-indicator")
                  .WithValue("&#187;")
                  .EndTag()
                  .EndTag()
                  .EndTag();

            string html = writer.End();
            Assert.AreEqual("<ul id=\"menu.Name\" class=\"sf-menu sf-vertical\">\r\n\t<li><a class=\"sf-with-ul\" href=\"#\">Link 1<span class=\"sf-sub-indicator\">&#187;</span></a></li>\r\n</ul>",
                html);
        }
        [TestMethod]
        public void WriteMenu_WithTextWriter()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(builder);
            FluentHtmlTextWriter writer = FluentHtmlTextWriter.Begin(stringWriter);

            writer.BeginTag(HtmlTextWriterTag.Ul)
                  .WithStyle(HtmlTextWriterStyle.BorderWidth, "0px")
                  .WithAttribute(HtmlTextWriterAttribute.Id, "menu.Name")
                  .WithAttribute(HtmlTextWriterAttribute.Class, "sf-menu sf-vertical");
            writer.BeginTag(HtmlTextWriterTag.Li);
            writer.BeginTag(HtmlTextWriterTag.A)
                  .WithStyle("border-width", "0px")
                  .WithAttribute(HtmlTextWriterAttribute.Class, "sf-with-ul")
                  .WithAttribute(HtmlTextWriterAttribute.Href, "#")
                  .WithValue("Link 1");
            writer.WriteTag(HtmlTextWriterTag.Span)
                  .WithAttribute(HtmlTextWriterAttribute.Class, "sf-sub-indicator")
                  .WithValue("&#187;");
            writer.EndTag();
            writer.EndTag();
            writer.EndTag();
            writer.Flush();

            string html = builder.ToString();
            Assert.AreEqual("<ul id=\"menu.Name\" class=\"sf-menu sf-vertical\" style=\"border-width:0px;\">\r\n\t<li><a class=\"sf-with-ul\" href=\"#\" style=\"border-width:0px;\">Link 1<span class=\"sf-sub-indicator\">&#187;</span></a></li>\r\n</ul>",
                html);
        }

        [TestMethod]
        [Ignore]
        public void WriteImg_Performance()
        {
            for (int i = 0; i < PERFORMANCE_TEST_RUNS; i++)
            {
                string imageRelativeUrl = "images/logo.jpg";
                string altText = "Company logo";

                StringBuilder htmlBuilder = new StringBuilder();
                HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(htmlBuilder));

                writer.AddAttribute(HtmlTextWriterAttribute.Src, imageRelativeUrl);
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, altText);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.Flush();

                string html = htmlBuilder.ToString();
            }
        }

        [TestMethod]
        [Ignore]
        public void WriteImgFluent_Performance()
        {
            for (int i = 0; i < PERFORMANCE_TEST_RUNS; i++)
            {
                string imageRelativeUrl = "images/logo.jpg";
                string altText = "Company logo";

                string html = FluentHtmlTextWriter.Begin()
                                               .WriteTag(HtmlTextWriterTag.Img)
                                               .WithAttribute(HtmlTextWriterAttribute.Src, imageRelativeUrl)
                                               .WithAttribute(HtmlTextWriterAttribute.Alt, altText)
                                               .End();
            }
        }
    }
}
