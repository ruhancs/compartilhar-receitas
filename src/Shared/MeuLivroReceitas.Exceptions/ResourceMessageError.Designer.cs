﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeuLivroReceitas.Exceptions {
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
    public class ResourceMessageError {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceMessageError() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MeuLivroReceitas.Exceptions.ResourceMessageError", typeof(ResourceMessageError).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to email is already in use.
        /// </summary>
        public static string EMAIL_ALREADY_EXIST {
            get {
                return ResourceManager.GetString("EMAIL_ALREADY_EXIST", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the email must be provided.
        /// </summary>
        public static string EMAIL_EMPTY {
            get {
                return ResourceManager.GetString("EMAIL_EMPTY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the email address must be valid.
        /// </summary>
        public static string EMAIL_INVALID {
            get {
                return ResourceManager.GetString("EMAIL_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the username must be provided.
        /// </summary>
        public static string EMPTY_USER {
            get {
                return ResourceManager.GetString("EMPTY_USER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to email or password wrong.
        /// </summary>
        public static string LOGIN_INVALID {
            get {
                return ResourceManager.GetString("LOGIN_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the password must be provided.
        /// </summary>
        public static string PASSWORD_EMPTY {
            get {
                return ResourceManager.GetString("PASSWORD_EMPTY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password must be at least 8 characters long.
        /// </summary>
        public static string PASSWORD_MIN_LENGTH {
            get {
                return ResourceManager.GetString("PASSWORD_MIN_LENGTH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the phone number must be provided.
        /// </summary>
        public static string PHONE_EMPTY {
            get {
                return ResourceManager.GetString("PHONE_EMPTY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the phone must be in the format XX X XXXX-XXXX.
        /// </summary>
        public static string PHONE_INVALID {
            get {
                return ResourceManager.GetString("PHONE_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal server error try again later.
        /// </summary>
        public static string UNKNOWN_ERROR {
            get {
                return ResourceManager.GetString("UNKNOWN_ERROR", resourceCulture);
            }
        }
    }
}
