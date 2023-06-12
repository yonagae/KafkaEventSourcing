using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFinAccountRepository _finAccountRepository;
        private readonly IBalanceByTransactionTypeRepository _balanceByTransactionTypeRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public EventHandler(IPostRepository postRepository, 
            ICommentRepository commentRepository,
            IFinAccountRepository finAccountRepository, 
            IBalanceByTransactionTypeRepository balanceByTransactionTypeRepository, 
            ITransactionTypeRepository transactionTypeRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _finAccountRepository = finAccountRepository;
            _balanceByTransactionTypeRepository = balanceByTransactionTypeRepository;
            _transactionTypeRepository = transactionTypeRepository;
        }

        public async Task On(PostCreatedEvent @event)
        {
            var post = new PostEntity
            {
                PostId = @event.Id,
                Author = @event.Author,
                DatePosted = @event.DatePosted,
                Message = @event.Message
            };

            await _postRepository.CreateAsync(post);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Message = @event.Message;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(PostLikedEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Likes++;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(CommentAddedEvent @event)
        {
            var comment = new CommentEntity
            {
                PostId = @event.Id,
                CommentId = @event.CommentId,
                CommentDate = @event.CommentDate,
                Comment = @event.Comment,
                Username = @event.Username,
                Edited = false
            };

            await _commentRepository.CreateAsync(comment);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            var comment = await _commentRepository.GetByIdAsync(@event.CommentId);

            if (comment == null) return;

            comment.Comment = @event.Comment;
            comment.Edited = true;
            comment.CommentDate = @event.EditDate;

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task On(CommentRemovedEvent @event)
        {
            await _commentRepository.DeleteAsync(@event.CommentId);
        }

        public async Task On(PostRemovedEvent @event)
        {
            await _postRepository.DeleteAsync(@event.Id);
        }

        public async Task On(NewFinAccountEvent @event)
        {
            var post = new FinAccountEntity
            {
                Id = @event.Id,
                Owner = @event.Owner,
                TotalBalance = 0,
                Balances = null
            };

            await _finAccountRepository.CreateAsync(post);
        }

        public async Task On(DebitFinAccountEvent @event)
        {
            var finAccount = await _finAccountRepository.GetByIdWithBalanceAsync(@event.Id);
            if (finAccount == null) return;
            finAccount.TotalBalance -= @event.DebitAmount;

            var transactionTypeBalance = finAccount.Balances.FirstOrDefault(x => x.TransactionType.Name == @event.TransactionType);

            if(transactionTypeBalance == null)
            {
                var transactionType = await _transactionTypeRepository.GetByNameAsync(finAccount.Id, @event.TransactionType);

                await _balanceByTransactionTypeRepository.CreateAsync(new BalanceByTransactionTypeEntity
                {
                    Balance = -@event.DebitAmount,
                    FinAccountId = finAccount.Id,
                    TransactionTypeId = transactionType.Id,
                    //FinAccount = finAccount,
                    //TransactionType = transactionType
                });
            }
            else
            {
                transactionTypeBalance.Balance -= @event.DebitAmount;
                await _balanceByTransactionTypeRepository.UpdateAsync(transactionTypeBalance);
            }

            await _finAccountRepository.UpdateAsync(finAccount);
        }

        public async Task On(CreditFinAccountEvent @event)
        {
            var finAccount = await _finAccountRepository.GetByIdWithBalanceAsync(@event.Id);
            if (finAccount == null) return;
            finAccount.TotalBalance += @event.CreditAmount;

            await _finAccountRepository.UpdateAsync(finAccount);
        }

        public async Task On(AddTransactionTypeEvent @event)
        {
            var transactionType = new TransactionTypeEntity
            {
                Id = Guid.NewGuid(),
                FinAccountId = @event.Id,
                Name = @event.TransactioTypeName
            };

            await _transactionTypeRepository.CreateAsync(transactionType);
        }
    }
}