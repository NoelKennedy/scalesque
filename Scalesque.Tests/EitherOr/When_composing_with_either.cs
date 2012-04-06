using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.EitherOr {

    public class Customer {
        public int CreditScore { get; set; }
        public bool GoldCustomer { get; set; }
    }

    public class LoanRequest {
        public double Value { get; set; }
        public Customer Customer { get; set; }
    }

    public enum LoanDeclinedReason{
        InsufficientCreditScore,
        RequiresGoldCustomer,
        LoanRequestTooHigh
    }

    public abstract class When_composing_with_either :UnitTestBase{

        //here are some business rules
        public Either<LoanDeclinedReason, LoanRequest> CheckCustomerCredit(LoanRequest request) {
            if (request.Customer.CreditScore > 8) return Either.Right(request);
            return Either.Left(LoanDeclinedReason.InsufficientCreditScore);
        }

        public Either<LoanDeclinedReason, LoanRequest> CheckGoldPrerequisiteCustomer(LoanRequest request) {
            if (request.Value < 10000) return Either.Right(request); //dont need gold status for less than 10,000
            if (request.Customer.GoldCustomer) return Either.Right(request);
            return Either.Left(LoanDeclinedReason.RequiresGoldCustomer);
        }

        public Either<LoanDeclinedReason, LoanRequest> CheckSillyAmount(LoanRequest request) {
            if (request.Value > 1000000)
                return Either.Left(LoanDeclinedReason.LoanRequestTooHigh);
            return Either.Right(request);
        }
    }

    public class When_a_gold_customer_applies: When_composing_with_either {
        private LoanRequest request;
        private bool result;

        public override void Given() {
            request = new LoanRequest {
                    Customer = new Customer { CreditScore = 10, GoldCustomer = true },
                    Value = 9000
                };
        }

        public override void Because() {
            Either<LoanDeclinedReason, LoanRequest> checkOne = CheckCustomerCredit(request);
            Either<LoanDeclinedReason, LoanRequest> checkTwo = checkOne.FlatMap(CheckGoldPrerequisiteCustomer); //Note:FlatMap instead of Map
            Either<LoanDeclinedReason, LoanRequest> checkThree = checkTwo.FlatMap(CheckSillyAmount);
            result = checkThree.Fold(failReason=>false,passedLoan=>true);
        }

        [Test]
        public void It_should_have_passed_the_loan_request() {
            result.Should().BeTrue();
        }

    }
}
