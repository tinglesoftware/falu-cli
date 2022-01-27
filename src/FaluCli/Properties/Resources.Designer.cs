﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Falu.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Falu.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The API Key provided or logged in user does not have permissions to perform this operation.
        ///Consult your Falu dashboard to confirm the permissions of the API key or ask your administrator to grant you permissions..
        /// </summary>
        internal static string Forbidden403Message {
            get {
                return ResourceManager.GetString("Forbidden403Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error Code: {0}.
        /// </summary>
        internal static string ProblemDetailsErrorCodeFormat {
            get {
                return ResourceManager.GetString("ProblemDetailsErrorCodeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error Detail: {0}.
        /// </summary>
        internal static string ProblemDetailsErrorDetailFormat {
            get {
                return ResourceManager.GetString("ProblemDetailsErrorDetailFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Request to Falu servers failed..
        /// </summary>
        internal static string RequestFailedHeader {
            get {
                return ResourceManager.GetString("RequestFailedHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Request Id: {0}.
        /// </summary>
        internal static string RequestIdFormat {
            get {
                return ResourceManager.GetString("RequestIdFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trace Identifier:.
        /// </summary>
        internal static string TraceIdentifierFormat {
            get {
                return ResourceManager.GetString("TraceIdentifierFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The API key provided cannot authenticate your request.
        ///Confirm you have provided the right value and that it matches any other related values.
        ///For example, do not use a key that belongs to another workspace or mix live and test mode keys..
        /// </summary>
        internal static string Unauthorized401ErrorMessage {
            get {
                return ResourceManager.GetString("Unauthorized401ErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unhandled exception: {0}.
        /// </summary>
        internal static string UnhandledExceptionFormat {
            get {
                return ResourceManager.GetString("UnhandledExceptionFormat", resourceCulture);
            }
        }
    }
}
