using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Extended.Collections.Tests
{
    public abstract class BaseTest
    {
        protected readonly ITestOutputHelper m_outputHelper;

        protected BaseTest(ITestOutputHelper testOutputHelper)
        {
            m_outputHelper = testOutputHelper;
        }

        protected void LogInfo(string message)
        {
            m_outputHelper.WriteLine(message);
        }
    }
}
