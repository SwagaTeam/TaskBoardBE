namespace AnalyticsService.BusinessLayer
{
    public class ForwardAccessTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _ctx;

        public ForwardAccessTokenHandler(IHttpContextAccessor ctx) => _ctx = ctx;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var bearer = _ctx.HttpContext?
                             .Request.Headers["Authorization"]
                             .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(bearer))
            {
                request.Headers.TryAddWithoutValidation("Authorization", bearer);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
