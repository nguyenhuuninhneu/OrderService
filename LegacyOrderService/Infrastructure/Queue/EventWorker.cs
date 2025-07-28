using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LegacyOrderService.Infrastructure.Queue;

public class EventWorker
{
    public string Id { get; }
    private readonly ChannelReader<object> _reader;
    private CancellationTokenSource? _cts;
    private Task? _task;

    public EventWorker(string id, ChannelReader<object> reader)
    {
        Id = id;
        _reader = reader;

        StartNew();
    }

    public void StartNew()
    {
        _cts = new CancellationTokenSource();
        _task = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var command = await _reader.ReadAsync(_cts.Token);

                try
                {
                    using var scope = Program.ServiceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(command, _cts.Token);
                }
                catch (Exception e) when (e is not OperationCanceledException)
                {
                    Console.WriteLine(e);
                }
            }

            _cts.Token.ThrowIfCancellationRequested();
        });
    }

}
