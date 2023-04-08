﻿using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Domain.Repositories;

//interface com as funçoes de escrita do usuario ex add
public interface IUserWriteOnlyRepository
{
    Task Add(Usuario user);
}