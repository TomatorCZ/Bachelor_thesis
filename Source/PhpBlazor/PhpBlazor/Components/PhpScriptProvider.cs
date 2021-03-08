using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class PhpScriptProvider : IComponent, IHandleAfterRender, IDisposable
    {
        #region IComponent
        void IComponent.Attach(RenderHandle renderHandle)
        {
            //throw new NotImplementedException();
        }

        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
        #endregion

        #region IHandleAfterRender
        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
