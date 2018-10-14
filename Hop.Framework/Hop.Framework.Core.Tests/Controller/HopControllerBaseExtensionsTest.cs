using Hop.Api.Server.Core.Controllers;
using Hop.Api.Server.Core.Dispatcher;
using Hop.Api.Server.Core.Response;
using Hop.Framework.Core.Log;
using Hop.Framework.Domain.Notification;
using Hop.Framework.Domain.Results;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Hop.Framework.Core.Tests.Controller
{
    [TestFixture]
    public class HopControllerBaseExtensionsTest
    {
        private const string MsgErroPadrao = "Mensagem padrão";
        private const string DetailErroPadrao = "ChaveErro";
        private const string DetalheMsgErroPadrao = "Detalhe " + MsgErroPadrao;

        [Test]
        public void ReturnOkResponse_Returns_Content()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnOkResponse(new RetornoTest()) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<RetornoTest>;
            Assert.IsNotNull(value);
            Assert.IsNotNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.OK);

            var retornoTest = value.Content;
            Assert.IsTrue(retornoTest.Chave == RetornoTest.DefaultChave);
            Assert.IsTrue(retornoTest.Descricao == RetornoTest.DefaultDescricao);
        }

        [Test]
        public void ReturnNoContentResponse_Returns_Nothing()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnNoContentResponse() as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public void ReturnNotFoundResponse_With_ErrorMessage()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnNotFoundResponse(MsgErroPadrao, DetalheMsgErroPadrao) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.NotFound);
            Assert.IsTrue(value.Messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetalheMsgErroPadrao);
        }

        [Test]
        public void ReturnNotFoundResponse_With_Exception()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnNotFoundResponse(new Exception(MsgErroPadrao, new Exception(DetalheMsgErroPadrao))) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.NotFound);
            Assert.IsTrue(value.Messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetalheMsgErroPadrao);
        }

        [Test]
        public void ReturnBadRequestResponse_With_ErrorMessage()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnBadRequestResponse(MsgErroPadrao, DetalheMsgErroPadrao) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(value.Messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetalheMsgErroPadrao);
        }

        [Test]
        public void ReturnBadRequestResponse_With_Exception()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnBadRequestResponse(new Exception(MsgErroPadrao, new Exception(DetalheMsgErroPadrao))) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(value.Messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetalheMsgErroPadrao);
        }

        [Test]
        public void ReturnInternalServerErrorResponse_With_ErrorMessage()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnInternalServerErrorResponse(MsgErroPadrao, DetalheMsgErroPadrao) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.InternalServerError);
            Assert.IsTrue(value.Messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetalheMsgErroPadrao);
        }

        [Test]
        public void ReturnInternalServerErrorResponse_With_Exception()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var result = controller.ReturnInternalServerErrorResponse(new Exception(MsgErroPadrao, new Exception(DetalheMsgErroPadrao))) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsNull(value.Content);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.InternalServerError);
            Assert.IsTrue(value.Messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetalheMsgErroPadrao);
        }

        [Test]
        public void ReturnResponseFromResult_OK_Returns_Content()
        {
            const string objContentString = "Valor do result é esta string";
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var resultObj = Result.Ok;
            resultObj.AddValue(objContentString);

            var result = controller.ReturnResponseFromResult(resultObj) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.OK);

            var valueAsString = value.Content as string;
            Assert.IsNotNull(valueAsString);
            Assert.IsTrue(valueAsString.Equals(objContentString, StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void ReturnResponseFromTypedResult_OK_Returns_Content()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var resultObj = new Result<RetornoTest>(new RetornoTest());
            var result = controller.ReturnResponseFromResult(resultObj) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<RetornoTest>;
            Assert.IsNotNull(value);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.OK);

            Assert.IsNotNull(value.Content);
            Assert.IsTrue(value.Content.Chave == RetornoTest.DefaultChave);
            Assert.IsTrue(value.Content.Descricao == RetornoTest.DefaultDescricao);
        }

        [Test]
        public void ReturnResponseFromResult_ERROR_Returns_Content()
        {
            const string objContentString = "Valor do result é esta string";
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var resultObj = Result.Error;
            resultObj.AddValue(objContentString);
            resultObj.AddMessages(new List<DomainNotification>()
            {
                new DomainNotification(DetailErroPadrao, MsgErroPadrao)
            });

            var result = controller.ReturnResponseFromResult(resultObj) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<object>;
            Assert.IsNotNull(value);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.BadRequest);

            var valueAsString = value.Content as string;
            Assert.IsNotNull(valueAsString);
            Assert.IsTrue(valueAsString.Equals(objContentString, StringComparison.InvariantCultureIgnoreCase));

            var messages = value.Messages;
            Assert.IsTrue(messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetailErroPadrao);
        }

        [Test]
        public void ReturnResponseFromTypedResult_ERROR_Returns_Content()
        {
            var logProvider = Substitute.For<ILogProvider>();
            var dispatcher = Substitute.For<IDispatcher>();
            var controller = new ControllerTest(logProvider, dispatcher);

            var resultObj = new Result<RetornoTest>(new RetornoTest(), false);
            resultObj.AddMessages(new List<DomainNotification>()
            {
                new DomainNotification(DetailErroPadrao, MsgErroPadrao)
            });

            var result = controller.ReturnResponseFromResult(resultObj) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var value = result.Value as HopApiResponse<RetornoTest>;
            Assert.IsNotNull(value);
            Assert.IsTrue(value.StatusCode == HttpStatusCode.BadRequest);

            Assert.IsNotNull(value.Content);
            Assert.IsTrue(value.Content.Chave == RetornoTest.DefaultChave);
            Assert.IsTrue(value.Content.Descricao == RetornoTest.DefaultDescricao);

            var messages = value.Messages;
            Assert.IsTrue(messages.Length == 1);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Content == MsgErroPadrao);
            Assert.IsTrue(value.Messages.FirstOrDefault()?.Detail == DetailErroPadrao);
        }
    }

    public class ControllerTest : HopControllerBase
    {
        public ControllerTest(ILogProvider logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
        {
        }
    }

    public class RetornoTest
    {
        public const long DefaultChave = 123;
        public const string DefaultDescricao = "Teste";

        public long Chave { get; }
        public string Descricao { get; }

        public RetornoTest() : this(DefaultChave, DefaultDescricao)
        {

        }

        public RetornoTest(long chave, string descricao)
        {
            Chave = chave;
            Descricao = descricao;
        }
    }
}
