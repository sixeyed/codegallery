using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Linq.Expressions;
using System.IO;

namespace FluentHtmlTextWriter
{
    /// <summary>
    /// Fluent interface for compiling HTML
    /// </summary>
    public class FluentHtmlTextWriter
    {
        #region Private instance fields

        private StringBuilder _builder;
        private HtmlTextWriter _writer;
        private Action<HtmlTextWriter> _startAction;
        private Action<HtmlTextWriter> _endAction;
        private List<Action<HtmlTextWriter>> _prefixActions = new List<Action<HtmlTextWriter>>();
        private List<Action<HtmlTextWriter>> _infixActions = new List<Action<HtmlTextWriter>>();

        #endregion

        #region Public static methods

        /// <summary>
        /// Creates a <see cref="FluentHtmlTextWriter"/> to render HTML output to a string
        /// </summary>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public static FluentHtmlTextWriter Begin()
        {
            return new FluentHtmlTextWriter();
        }

        /// <summary>
        /// Creates a <see cref="FluentHtmlTextWriter"/> to render HTML output to the supplied TextWriter
        /// </summary>
        /// <param name="writer">Writer to render HTML to</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public static FluentHtmlTextWriter Begin(TextWriter writer)
        {
            return new FluentHtmlTextWriter(writer);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// Initialises an internal TextWriter, so the <see cref="FluentHtmlTextWriter"/>
        /// can be used to retrieve a string output, rather than writing to a stream
        /// </remarks>
        public FluentHtmlTextWriter()
        {
            _builder = new StringBuilder();
            _writer = new HtmlTextWriter(new StringWriter(_builder));
        }

        /// <summary>
        /// Constructor with supplied TextWriter
        /// </summary>
        /// <param name="writer">Writer to render HTML to</param>
        public FluentHtmlTextWriter(TextWriter writer)
        {
            _writer = new HtmlTextWriter(writer);
        }

        #endregion

        #region Private instance methods

        private void AddPrefix(Action<HtmlTextWriter> action)
        {
            _prefixActions.Add(action);
        }

        private void Add(Action<HtmlTextWriter> action)
        {
            _infixActions.Add(action);
        }

        private void SetStart(Action<HtmlTextWriter> action)
        {
            _startAction = action;
        }

        private void SetEnd(Action<HtmlTextWriter> action)
        {
            _endAction = action;
        }

        private void FlushActions()
        {
            if (_startAction != null)
            {
                foreach (Action<HtmlTextWriter> action in _prefixActions)
                {
                    action(_writer);
                }
                _startAction(_writer);
                foreach (Action<HtmlTextWriter> action in _infixActions)
                {
                    action(_writer);
                }
                _startAction = null;
                _prefixActions.Clear();
                _infixActions.Clear();
            }
            if (_endAction != null)
            {
                _endAction(_writer);
                _endAction = null;
            }

        }

        #endregion

        #region Public instance methods

        /// <summary>
        /// Writes an HTML tag
        /// </summary>
        /// <param name="tagKey">Type of tag to write</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter WriteTag(HtmlTextWriterTag tagKey)
        {
            FlushActions();
            BeginTag(tagKey);
            SetEnd(f => f.RenderEndTag());
            return this;
        }

        /// <summary>
        /// Adds an attribute to the current tag
        /// </summary>
        /// <param name="key">Type of attribute to write</param>
        /// <param name="value">Attribute value</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter WithAttribute(HtmlTextWriterAttribute key, string value)
        {
            AddPrefix(f => f.AddAttribute(key, value));
            return this;
        }

        /// <summary>
        /// Adds an attribute to the current tag
        /// </summary>
        /// <param name="name">Name of attribute to write</param>
        /// <param name="value">Attribute value</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter WithAttribute(string name, string value)
        {
            AddPrefix(f => f.AddAttribute(name, value));
            return this;
        }

        /// <summary>
        /// Adds a style attribute to the current tag
        /// </summary>
        /// <param name="key">Style attribute to write</param>
        /// <param name="value">Style attribute value</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter WithStyle(HtmlTextWriterStyle key, string value)
        {
            AddPrefix(f => f.AddStyleAttribute(key, value));
            return this;
        }

        /// <summary>
        /// Adds a style attribute to the current tag
        /// </summary>
        /// <param name="key">Style attribute to write</param>
        /// <param name="value">Style attribute value</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter WithStyle(string key, string value)
        {
            AddPrefix(f => f.AddStyleAttribute(key, value));
            return this;
        }

        /// <summary>
        /// Writes the content value of the current tag
        /// </summary>
        /// <param name="value">Content value</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter WithValue(string value)
        {
            Add(f => f.Write(value));
            return this;
        }

        /// <summary>
        /// Starts writing an HTML tag
        /// </summary>
        /// <param name="tagKey">Type of tag to write</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter BeginTag(HtmlTextWriterTag tagKey)
        {
            FlushActions();
            SetStart(f => f.RenderBeginTag(tagKey));
            return this;
        }

        /// <summary>
        /// Closes an HTMLtag
        /// <param name="tagKey">Type of tag to write</param>
        /// <returns><see cref="FluentHtmlTextWriter"/></returns>
        public FluentHtmlTextWriter EndTag()
        {
            FlushActions();
            _writer.RenderEndTag();
            return this;
        }

        /// <summary>
        /// Flushes the writer, writing buffered content to output stream
        /// </summary>
        public void Flush()
        {
            FlushActions();
            _writer.Flush();
        }

        /// <summary>
        /// Returns the rendered HTML
        /// </summary>
        /// <remarks>
        /// Only applies if the <see cref="FluentHtmlTextWriter"/> was created without
        /// a supplied TextWriter
        /// </remarks>
        /// <returns>HTML string</returns>
        public string End()
        {
            Flush();
            string html = string.Empty;
            if (_builder != null)
            {
                html = _builder.ToString();
            }
            return html;
        }

        #endregion
    }
}
