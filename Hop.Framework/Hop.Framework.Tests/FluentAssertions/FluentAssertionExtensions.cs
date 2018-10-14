using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Linq;

namespace Hop.Framework.UnitTests.FluentAssertions
{
    public static class FluentAssertionExtensions
    {
        public static void HaveEquivalentMembers<TExpectation>(this ObjectAssertions obj,
            TExpectation expectation)
        {
            obj.BeEquivalentTo(expectation, opt => opt.ComparingByMembers<TExpectation>());
        }

        public static void HaveEquivalentMembers<TExpectation>(this ObjectAssertions obj,
            TExpectation expectation,
            Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
            string because = "",
            params object[] becauseArgs)
        {
            obj.BeEquivalentTo(expectation, opt => config.Invoke(opt.ComparingByMembers<TExpectation>()), because, becauseArgs);
        }

        public static void HaveValidator<TValidator>(this GenericCollectionAssertions<IValidationRule> assertion)
        {
            assertion
                .Subject
                .SelectMany(x => x.Validators)
                .Where(v => v is ChildValidatorAdaptor)
                .Cast<ChildValidatorAdaptor>()
                .Select(x => x.ValidatorType)
                .Should()
                .Contain(typeof(TValidator));
        }
    }
}
