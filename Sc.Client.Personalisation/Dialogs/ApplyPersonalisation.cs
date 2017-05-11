namespace Sc.Client.Personalisation.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using Constants;
    using Rules;
    using Sitecore;
    using Sitecore.CodeDom.Scripts;
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;
    using Sitecore.Diagnostics;
    using Sitecore.Extensions.StringExtensions;
    using Sitecore.Globalization;
    using Sitecore.Reflection;
    using Sitecore.Rules;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.Pages;
    using Sitecore.Web.UI.Sheer;

    public class ApplyPersonalisation : DialogForm
    {
        public Scrollbox Conditions
        {
            get;
            set;
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            var form = global::Sitecore.Context.ClientPage.ClientRequest.Form;
            var checks = new List<string>();
            foreach (string key in form.AllKeys.Where(t => t != null && t.StartsWith("check_")))
            {
                var selected = form[key];
                if (selected == "on")
                {
                    checks.Add(string.Format("{0}={1}", PersonalisationConstants.RequestPrefix + form[key.Replace("check", "request")], form[key.Replace("check", "value")]));
                }
            }
            if (checks.Any())
            {
                SheerResponse.SetDialogValue(string.Join("&", checks));
            }
            else
            {
                SheerResponse.SetDialogValue("none");
            }
            base.OnOK(sender, args);
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnPreRender(e);
            if (global::Sitecore.Context.ClientPage.IsEvent)
            {
                return;
            }
            var items =
                Client.ContentDatabase.GetItem(
                    "/sitecore/system/Settings/Rules/Definitions/Elements/Custom").Children
                    .Where(t => t.TemplateID.Equals(RuleIds.Condition));
            this.RenderConditions(items);
        }

        private void RenderConditions(IEnumerable<Item> items)
        {
            System.Web.UI.HtmlTextWriter innerOutput = new System.Web.UI.HtmlTextWriter(new System.IO.StringWriter());
            foreach (var item in items)
            {
                string itemText = GetItemText(item);
                if (!string.IsNullOrEmpty(itemText))
                {
                    var type = ItemScripts.GetGenericType<RuleContext>(item);
                    var rule = ReflectionUtil.CreateObject(type) as IPersonalisationRule;
                    if (rule != null)
                    {
                        var currentValue =
                            global::Sitecore.Context.ClientPage.Request.Cookies[PersonalisationConstants.RequestPrefix + rule.RequestName];
                        innerOutput.Write("<div style=\"padding:0px 0px 4px 24px\">");
                        innerOutput.Write(string.Format("<input type=\"checkbox\" id=\"check_{0}\" {1} />&nbsp;",
                            item.ID.Guid, currentValue == null ? string.Empty : "checked"));
                        innerOutput.Write(string.Format("<input type=\"hidden\" id=\"request_{0}\" value=\"{1}\" />",
                            item.ID.Guid, rule.RequestName));
                        RenderText(innerOutput, item.ID.Guid.ToString(), itemText, currentValue == null ? string.Empty : currentValue.Value);
                        innerOutput.Write("</div>");
                    }

                }
            }
            this.Conditions.InnerHtml = innerOutput.InnerWriter.ToString();
        }
        private static string GetItemText(Item item)
        {
            string text = item["Text"];
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
            Language language = LanguageManager.GetLanguage("en", item.Database);
            if (language != null)
            {
                Item item2 = item.Database.GetItem(item.ID, language);
                if (item2 != null)
                {
                    text = item2["Text"];
                    if (!string.IsNullOrEmpty(text))
                    {
                        return text;
                    }
                }
            }
            return string.Empty;
        }

        protected void Edit(string uid)
        {
            Assert.ArgumentNotNull(uid, "uid");
            var args = global::Sitecore.Context.ClientPage.CurrentPipelineArgs as ClientPipelineArgs;

            if (args.IsPostBack)
            {
                if (args.HasResult && args.Result != "-")
                {
                    var value = args.Result;
                    SheerResponse.SetInnerHtml("link_" + uid, value);
                    SheerResponse.SetAttribute("value_" + uid, "value", value);
                }
                return;
            }

            SheerResponse.Input("Enter value", "");
            args.WaitForPostBack();
        }

        private void RenderText(System.Web.UI.HtmlTextWriter output, string uid, string itemText, string currentValue)
        {
            itemText = System.Web.HttpUtility.HtmlEncode(itemText);
            var isConfigurable = Regex.Match(itemText, "\\[([^\\]])*\\]");
            if (isConfigurable.Success)
            {

                string[] configValues = isConfigurable.Value.Mid(1, isConfigurable.Value.Length - 2).Split(',');
                string property = configValues[0];

                string jsLink = StringUtil.EscapeQuote("Edit(\"" + uid + "\")");
                string linkText = property;
                if (configValues.Length >= 4)
                {
                    linkText = configValues[3];
                }
                if (!string.IsNullOrEmpty(currentValue))
                {
                    linkText = currentValue;
                }
                linkText = string.Format(
                        "<a id=\"link_{2}\" href=\"#\" onclick=\"javascript:return scForm.postRequest('','','','{0}')\" style=\"color:#e0b406;text-decoration:underline\">{1}</a>",
                        jsLink, linkText, uid);
                itemText = itemText.Replace(isConfigurable.Value, linkText);
                WriteHiddenValue(output, uid, currentValue);
            }
            else
            {
                WriteHiddenValue(output, uid, "1");
            }

            output.Write(itemText);
        }

        private void WriteHiddenValue(HtmlTextWriter output, string uid, string value)
        {
            output.Write(string.Format("<input type=\"hidden\" id=\"value_{0}\" value=\"{1}\" />", uid, value));
        }
    }
}