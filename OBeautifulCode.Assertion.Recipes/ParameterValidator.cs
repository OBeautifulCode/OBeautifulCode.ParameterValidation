﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidator.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Assertion.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Assertion.Recipes
{
    using System;
    using System.Collections;

    /// <summary>
    /// Extension methods.
    /// </summary>
#if !OBeautifulCodeAssertionRecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Assertion.Recipes", "See package version number")]
    internal
#else
    public
#endif
        static class ParameterValidator
    {
        /// <summary>
        /// Specifies the name of the parameter.
        /// </summary>
        /// <typeparam name="TParameterValue">The type of parameter value.</typeparam>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>
        /// The parameter to validate.
        /// </returns>
        public static Parameter Named<TParameterValue>(
            [ValidatedNotNull] this TParameterValue value,
            string name)
        {
            var parameter = value as Parameter;
            if (parameter != null)
            {
                ThrowImproperUseOfFrameworkIfDetected(parameter, ParameterShould.NotExist);
            }

            var result = new Parameter
            {
                Value = value,
                Name = name,
                HasBeenNamed = true,
                ValueType = typeof(TParameterValue),
            };

            return result;
        }

        /// <summary>
        /// Initializes a parameter for validation.
        /// </summary>
        /// <typeparam name="TParameterValue">The type of parameter value.</typeparam>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>
        /// The parameter to validate.
        /// </returns>
        public static Parameter Must<TParameterValue>(
            [ValidatedNotNull] this TParameterValue value)
        {
            // it a parameter itself? pass-thru
            var parameter = value as Parameter;
            if (parameter != null)
            {
                ThrowImproperUseOfFrameworkIfDetected(parameter, ParameterShould.BeNamed, ParameterShould.NotBeMusted, ParameterShould.NotBeEached, ParameterShould.NotBeValidated);
                parameter.HasBeenMusted = true;
                return parameter;
            }

            var valueType = typeof(TParameterValue);

            if (!ReferenceEquals(value, null))
            {
                // is anonymous type?
                // https://stackoverflow.com/a/15273117/356790
                if (valueType.Namespace == null)
                {
                    // with one property?  that's the parameter we are trying to validate.
                    var properties = valueType.GetProperties();
                    if (properties.Length == 1)
                    {
                        var parameterInAnonymousObject = new Parameter
                        {
                            Value = properties[0].GetValue(value, null),
                            Name = properties[0].Name,
                            ValueType = properties[0].PropertyType,
                            HasBeenMusted = true,
                        };

                        return parameterInAnonymousObject;
                    }
                    else
                    {
                        ThrowImproperUseOfFramework();
                    }
                }
            }

            var directParameter = new Parameter
            {
                Value = value,
                HasBeenMusted = true,
                ValueType = valueType,
            };

            return directParameter;
        }

        /// <summary>
        /// Specifies that the validations should be applied by iterating
        /// over the <see cref="IEnumerable"/> parameter value.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// The parameter who's value should be iterated over when applying validations.
        /// </returns>
        public static Parameter Each(
            [ValidatedNotNull] this Parameter parameter)
        {
            ThrowImproperUseOfFrameworkIfDetected(parameter, ParameterShould.BeMusted, ParameterShould.NotBeEached);
            parameter.HasBeenEached = true;
            return parameter;
        }

        /// <summary>
        /// Specifies another validation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// The parameter to validate.
        /// </returns>
        public static Parameter And(
            [ValidatedNotNull] this Parameter parameter)
        {
            ThrowImproperUseOfFrameworkIfDetected(parameter, ParameterShould.BeMusted, ParameterShould.BeValidated);
            return parameter;
        }

        /// <summary>
        /// Throws an exception if an improper use of the framework is detected.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterShoulds">Specifies what should or should not be true about the parameter.</param>
        internal static void ThrowImproperUseOfFrameworkIfDetected(
            [ValidatedNotNull] Parameter parameter,
            params ParameterShould[] parameterShoulds)
        {
            bool shouldThrow = false;
            if (parameter == null)
            {
                shouldThrow = true;
            }
            else if (parameter.ValueType == null)
            {
                shouldThrow = true;
            }
            else
            {
                foreach (var parameterShould in parameterShoulds)
                {
                    switch (parameterShould)
                    {
                        case ParameterShould.NotExist:
                            shouldThrow = true;
                            break;
                        case ParameterShould.BeNamed:
                            shouldThrow = !parameter.HasBeenNamed;
                            break;
                        case ParameterShould.NotBeNamed:
                            shouldThrow = parameter.HasBeenNamed;
                            break;
                        case ParameterShould.BeMusted:
                            shouldThrow = !parameter.HasBeenMusted;
                            break;
                        case ParameterShould.NotBeMusted:
                            shouldThrow = parameter.HasBeenMusted;
                            break;
                        case ParameterShould.BeEached:
                            shouldThrow = !parameter.HasBeenEached;
                            break;
                        case ParameterShould.NotBeEached:
                            shouldThrow = parameter.HasBeenEached;
                            break;
                        case ParameterShould.BeValidated:
                            shouldThrow = !parameter.HasBeenValidated;
                            break;
                        case ParameterShould.NotBeValidated:
                            shouldThrow = parameter.HasBeenValidated;
                            break;
                        default:
                            shouldThrow = true;
                            break;
                    }

                    if (shouldThrow)
                    {
                        break;
                    }
                }
            }

            if (shouldThrow)
            {
                ThrowImproperUseOfFramework();
            }
        }

        /// <summary>
        /// Throws an exception to inform the caller that the framework is being used improperly.
        /// </summary>
        /// <param name="message">Optional message to prepend.</param>
        internal static void ThrowImproperUseOfFramework(
            string message = null)
        {
            // We throw a InvalidOperationException rather than an ArgumentException so that this category of
            // problem (inproper use of the framework), can be clearly differentiated from a validation failure
            // (which will throw ArgumentException or some derivative) by the caller.
            // If we didn't throw here:
            //   - if parameter == null then NullReferenceException would be thrown soon after, when the parameter
            //     gets used, except that it would not have a nice message like the one below.  In addition, we would
            //     have to sprinkle Code Analysis suppressions throughout the project, for CA1062.
            //   - if parameter != null then the user doesn't understand how the framework is designed to be used
            //     and what the framework's limiations are.  Some negative outcome might occur (throwing when
            //     not expected or not throwing when expected).
            message = message == null ? ParameterValidation.ImproperUseOfFrameworkExceptionMessage : message + "  " + ParameterValidation.ImproperUseOfFrameworkExceptionMessage;
            throw new InvalidOperationException(message);
        }
    }
}
