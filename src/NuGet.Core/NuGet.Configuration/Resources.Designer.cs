﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NuGet.Configuration {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NuGet.Configuration.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Argument cannot be null, empty, or whitespace only..
        /// </summary>
        internal static string Argument_Cannot_Be_Null_Empty_Or_WhiteSpaceOnly {
            get {
                return ResourceManager.GetString("Argument_Cannot_Be_Null_Empty_Or_WhiteSpaceOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value cannot be null or empty string..
        /// </summary>
        internal static string Argument_Cannot_Be_Null_Or_Empty {
            get {
                return ResourceManager.GetString("Argument_Cannot_Be_Null_Or_Empty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attribute &apos;{0}&apos; is not allowed in element &apos;{1}&apos;..
        /// </summary>
        internal static string AttributeNotAllowed {
            get {
                return ResourceManager.GetString("AttributeNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The attribute &apos;{0}&apos; has an unallowed value &apos;{1}&apos; in element &apos;{2}&apos;..
        /// </summary>
        internal static string AttributeValueNotAllowed {
            get {
                return ResourceManager.GetString("AttributeValueNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The item passed to the Update method cannot refer to a different item than the one being updated..
        /// </summary>
        internal static string CannotUpdateDifferentItems {
            get {
                return ResourceManager.GetString("CannotUpdateDifferentItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to update setting since it is in a machine-wide NuGet.Config..
        /// </summary>
        internal static string CannotUpdateMachineWide {
            get {
                return ResourceManager.GetString("CannotUpdateMachineWide", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot update the node of a setting..
        /// </summary>
        internal static string CannotUpdateNode {
            get {
                return ResourceManager.GetString("CannotUpdateNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to update setting since it is in an uneditable config file..
        /// </summary>
        internal static string CannotUpdateReadOnlyConfig {
            get {
                return ResourceManager.GetString("CannotUpdateReadOnlyConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are multiple client certificate configurations associated with the same package source(s): {0}.
        /// </summary>
        internal static string ClientCertificateDuplicateConfiguration {
            get {
                return ResourceManager.GetString("ClientCertificateDuplicateConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Credentials item must have username and password..
        /// </summary>
        internal static string CredentialsItemMustHaveUsernamePassword {
            get {
                return ResourceManager.GetString("CredentialsItemMustHaveUsernamePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Package source &apos;{0}&apos; has already been defined previously..
        /// </summary>
        internal static string Error_DuplicatePackageSource {
            get {
                return ResourceManager.GetString("Error_DuplicatePackageSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Encryption is not supported on non-Windows platforms..
        /// </summary>
        internal static string Error_EncryptionUnsupported {
            get {
                return ResourceManager.GetString("Error_EncryptionUnsupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The attribute {0}-{1} is not valid..
        /// </summary>
        internal static string Error_InvalidAttribute {
            get {
                return ResourceManager.GetString("Error_InvalidAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Package source &apos;{0}&apos; must have at least one package pattern..
        /// </summary>
        internal static string Error_ItemNeedsAtLeastOnePackagePattern {
            get {
                return ResourceManager.GetString("Error_ItemNeedsAtLeastOnePackagePattern", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Package source &apos;{0}&apos; must have at least one package pattern. Path: &apos;{1}&apos;.
        /// </summary>
        internal static string Error_ItemNeedsAtLeastOnePackagePatternWithPath {
            get {
                return ResourceManager.GetString("Error_ItemNeedsAtLeastOnePackagePatternWithPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot merge two different sections..
        /// </summary>
        internal static string Error_MergeTwoDifferentSections {
            get {
                return ResourceManager.GetString("Error_MergeTwoDifferentSections", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A credentials item must have only one Password or ClearTextPassword entry..
        /// </summary>
        internal static string Error_MoreThanOnePassword {
            get {
                return ResourceManager.GetString("Error_MoreThanOnePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A credentials item must have only one Username entry..
        /// </summary>
        internal static string Error_MoreThanOneUsername {
            get {
                return ResourceManager.GetString("Error_MoreThanOneUsername", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A credentials item must have only one ValidAuthenticationTypes entry..
        /// </summary>
        internal static string Error_MoreThanOneValidAuthenticationTypes {
            get {
                return ResourceManager.GetString("Error_MoreThanOneValidAuthenticationTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Certificate for the package source &apos;{0}&apos; was not found in &apos;{1}.{2}&apos; storage by &apos;{3}&apos; criteria with &apos;{4}&apos; value..
        /// </summary>
        internal static string Error_StoreCertCertificateNotFound {
            get {
                return ResourceManager.GetString("Error_StoreCertCertificateNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password and ClearTextPassword cannot be used at the same time..
        /// </summary>
        internal static string FileCertItemPasswordAndClearTextPasswordAtSameTime {
            get {
                return ResourceManager.GetString("FileCertItemPasswordAndClearTextPasswordAtSameTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client certificate configuration password for the package source &apos;{0}&apos; cannot be decrypted.
        /// </summary>
        internal static string FileCertItemPasswordCannotBeDecrypted {
            get {
                return ResourceManager.GetString("FileCertItemPasswordCannotBeDecrypted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A fileCert path specified a file that does not exist..
        /// </summary>
        internal static string FileCertItemPathFileNotExist {
            get {
                return ResourceManager.GetString("FileCertItemPathFileNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A fileCert item path was not set..
        /// </summary>
        internal static string FileCertItemPathFileNotSet {
            get {
                return ResourceManager.GetString("FileCertItemPathFileNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File &apos;{0}&apos; does not exist..
        /// </summary>
        internal static string FileDoesNotExist {
            get {
                return ResourceManager.GetString("FileDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; cannot be called on a NullSettings. This may be caused on account of insufficient permissions to read or write to &quot;%AppData%\NuGet\NuGet.config&quot;..
        /// </summary>
        internal static string InvalidNullSettingsOperation {
            get {
                return ResourceManager.GetString("InvalidNullSettingsOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The item does not exist in the {0} section..
        /// </summary>
        internal static string ItemDoesNotExist {
            get {
                return ResourceManager.GetString("ItemDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing required attribute &apos;{0}&apos; in element &apos;{1}&apos;..
        /// </summary>
        internal static string MissingRequiredAttribute {
            get {
                return ResourceManager.GetString("MissingRequiredAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; must contain an absolute path &apos;{1}&apos;..
        /// </summary>
        internal static string MustContainAbsolutePath {
            get {
                return ResourceManager.GetString("MustContainAbsolutePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} item should not have any attributes and it was found with {1}..
        /// </summary>
        internal static string NoAttributesAllowed {
            get {
                return ResourceManager.GetString("NoAttributesAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no NuGet.Config that could be used for writing settings. Please create one at the desired location and restart the client..
        /// </summary>
        internal static string NoWritteableConfig {
            get {
                return ResourceManager.GetString("NoWritteableConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Owners item must have at least one owner..
        /// </summary>
        internal static string OwnersItemMustHaveAtLeastOneOwner {
            get {
                return ResourceManager.GetString("OwnersItemMustHaveAtLeastOneOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Owners item must only have text content and cannot be empty..
        /// </summary>
        internal static string OwnersMustOnlyHaveContent {
            get {
                return ResourceManager.GetString("OwnersMustOnlyHaveContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} cannot be null or empty..
        /// </summary>
        internal static string PropertyCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("PropertyCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The repository item with name &apos;{0}&apos; and service index &apos;{1}&apos; has more than one owners item in it..
        /// </summary>
        internal static string RepositoryMustHaveOneOwners {
            get {
                return ResourceManager.GetString("RepositoryMustHaveOneOwners", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The section &apos;{0}&apos; does not exist in the settings..
        /// </summary>
        internal static string SectionDoesNotExist {
            get {
                return ResourceManager.GetString("SectionDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &apos;fileName&apos; to Settings must be just a file name and not a path..
        /// </summary>
        internal static string Settings_FileName_Cannot_Be_A_Path {
            get {
                return ResourceManager.GetString("Settings_FileName_Cannot_Be_A_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error parsing NuGet.Config. Element &apos;{0}&apos; cannot have descendant elements. Path: &apos;{1}&apos;..
        /// </summary>
        internal static string ShowError_CannotHaveChildren {
            get {
                return ResourceManager.GetString("ShowError_CannotHaveChildren", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: NuGet.Config has an invalid package source value &apos;{1}&apos;. Reason: {2}.
        /// </summary>
        internal static string ShowError_ConfigHasInvalidPackageSource {
            get {
                return ResourceManager.GetString("ShowError_ConfigHasInvalidPackageSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NuGet.Config is malformed. Path: &apos;{0}&apos;..
        /// </summary>
        internal static string ShowError_ConfigInvalidOperation {
            get {
                return ResourceManager.GetString("ShowError_ConfigInvalidOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NuGet.Config is not valid XML. Path: &apos;{0}&apos;..
        /// </summary>
        internal static string ShowError_ConfigInvalidXml {
            get {
                return ResourceManager.GetString("ShowError_ConfigInvalidXml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NuGet.Config does not contain the expected root element: &apos;configuration&apos;. Path: &apos;{0}&apos;..
        /// </summary>
        internal static string ShowError_ConfigRootInvalid {
            get {
                return ResourceManager.GetString("ShowError_ConfigRootInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to read NuGet.Config due to unauthorized access. Path: &apos;{0}&apos;..
        /// </summary>
        internal static string ShowError_ConfigUnauthorizedAccess {
            get {
                return ResourceManager.GetString("ShowError_ConfigUnauthorizedAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Text elements should not be empty..
        /// </summary>
        internal static string TextShouldNotBeEmpty {
            get {
                return ResourceManager.GetString("TextShouldNotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A trusted signer entry must have at least one certificate entry..
        /// </summary>
        internal static string TrustedSignerMustHaveCertificates {
            get {
                return ResourceManager.GetString("TrustedSignerMustHaveCertificates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected failure reading NuGet.Config. Path: &apos;{0}&apos;..
        /// </summary>
        internal static string Unknown_Config_Exception {
            get {
                return ResourceManager.GetString("Unknown_Config_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown hash algorithm is not supported..
        /// </summary>
        internal static string UnknownHashAlgorithmNotSupported {
            get {
                return ResourceManager.GetString("UnknownHashAlgorithmNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password decryption is not supported on .NET Core for this platform. The following feed uses an encrypted password: &apos;{0}&apos;. You can use a clear text password as a workaround..
        /// </summary>
        internal static string UnsupportedDecryptPassword {
            get {
                return ResourceManager.GetString("UnsupportedDecryptPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password encryption is not supported on .NET Core for this platform. The following feed try to use an encrypted password: &apos;{0}&apos;. You can use a clear text password as a workaround..
        /// </summary>
        internal static string UnsupportedEncryptPassword {
            get {
                return ResourceManager.GetString("UnsupportedEncryptPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Certificate entry has an unsupported hash algorithm: &apos;{0}&apos;..
        /// </summary>
        internal static string UnsupportedHashAlgorithm {
            get {
                return ResourceManager.GetString("UnsupportedHashAlgorithm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse config file because: {0} Path: &apos;{1}&apos;..
        /// </summary>
        internal static string UserSettings_UnableToParseConfigFile {
            get {
                return ResourceManager.GetString("UserSettings_UnableToParseConfigFile", resourceCulture);
            }
        }
    }
}
