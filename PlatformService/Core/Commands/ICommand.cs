using MediatR;

public interface ICommand<out TRepsonse> : IRequest<TRepsonse> { }