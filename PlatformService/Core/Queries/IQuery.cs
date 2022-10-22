using MediatR;

public interface IQuery<out TRepsonse> : IRequest<TRepsonse> { }
