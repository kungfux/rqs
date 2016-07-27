using Fuse.ViewModels;
using Xunit;

namespace UnitTests.ViewModels
{
    public class LambdaCommandTests : UnitTest
    {
        [Fact]
        public void EnabledLambdaCommand_ExecutesLambdaAndAlwaysEnabled()
        {
            var varToUpdate = false;
            var cmd = LambdaCommand.From(() => varToUpdate = true);

            Assert.True(cmd.CanExecute(null));
            cmd.Execute(null);
            Assert.True(cmd.CanExecute(null));
            Assert.True(varToUpdate);
        }

        [Fact]
        public void ParametrizedEnabledLambdaCommand_UsesParameterAndAlwaysEnabled()
        {
            const string updateResult = "updated";
            var varToUpdate = string.Empty;
            var cmd = LambdaCommand.From(parameter => varToUpdate = parameter.ToString());

            Assert.True(cmd.CanExecute(null));
            cmd.Execute(updateResult);
            Assert.True(cmd.CanExecute(null));
            Assert.Equal(varToUpdate, updateResult);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LambdaCommand_ExecutesLambda(bool canExecute)
        {
            var varToUpdate = false;
            var cmd = LambdaCommand.From(() => varToUpdate = true).CanExecuteIf(() => canExecute);

            Assert.Equal(cmd.CanExecute(null), canExecute);
            cmd.Execute(null);
            Assert.Equal(cmd.CanExecute(null), canExecute);
            Assert.True(varToUpdate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ParametrizedLambdaCommand_UsesParameter(bool canExecute)
        {
            const string updateResult = "updated";
            var varToUpdate = string.Empty;
            var cmd = LambdaCommand.From(parameter => varToUpdate = parameter.ToString())
                .CanExecuteIf(parameter => parameter != null && canExecute);

            Assert.Equal(cmd.CanExecute(updateResult), canExecute);
            cmd.Execute(updateResult);
            Assert.Equal(cmd.CanExecute(updateResult), canExecute);
            Assert.Equal(varToUpdate, updateResult);

            if (canExecute)
                Assert.False(cmd.CanExecute(null));
        }

        [Fact]
        public void LambdaCommand_CanExecuteDependsOnLocalField_ReturnsProperValue()
        {
            var varToUpdate = false;
            var cmd = LambdaCommand.From(() => varToUpdate = true).CanExecuteIf(() => varToUpdate);

            Assert.False(cmd.CanExecute(null));
            cmd.Execute(null);
            Assert.True(cmd.CanExecute(null));
        }
    }
}
