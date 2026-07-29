using System;
using System.ComponentModel.DataAnnotations;

namespace Pds.FundingClaim.Repositories.Enums.Support
{
    /// <summary>
    /// Extensions to the enums in namespace Pds.FundingClaim.Repositories.Enums.Support.s/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the <see cref="FundingClaimType"/> that represents the corporate version.
        /// </summary>
        /// <param name="claimTypeName">The ClaimTypeName string from the schema funding claim to match on.</param>
        /// <returns>The domain specific version of the type.</returns>
        public static FundingClaimType ToFundingClaimType(this string claimTypeName)
        {
            return Enum.Parse<FundingClaimType>(claimTypeName.ToUpper().Replace(" ", string.Empty));
        }

        /// <summary>
        /// Gets the <see cref="ReconciliationType"/> that represents the corporate version.
        /// </summary>
        /// <param name="reconciliationTypeName">The ReconciliationTypeName string from the schema funding claim to match on.</param>
        /// <returns>The domain specific version of the type.</returns>
        public static ReconciliationType ToReconciliationType(this string reconciliationTypeName)
        {
            return Enum.Parse<ReconciliationType>(reconciliationTypeName.ToUpper().Replace(" ", string.Empty));
        }

        /// <summary>
        /// Gets the name of an enum's DisplayAttribute<see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="TEnum">Represents any enum type.</typeparam>
        /// <param name="value">The Enum value for which we are getting the Display name.</param>
        /// <returns>String that is the name of the enum's Display Attribute.</returns>
        public static string GetDisplayName<TEnum>(this TEnum value)
            where TEnum : struct
        {
            var type = value.GetType();
            var members = type.GetMember(value.ToString());

            if (members.Length == 0)
            {
                throw new ArgumentException($"error '{value}' not found in type '{type.Name}'");
            }

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes.Length == 0)
            {
                throw new ArgumentException($"'{type.Name}.{value}' doesn't have DisplayAttribute");
            }

            var attribute = (DisplayAttribute)attributes[0];
            return attribute.GetName();
        }
    }
}