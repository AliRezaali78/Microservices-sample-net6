using MediatR;

public interface IQueryHandler<in TQuery, TRepsonse> : IRequestHandler<TQuery, TRepsonse>
    where TQuery : IQuery<TRepsonse>
{ }
