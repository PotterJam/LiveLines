﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Lines;

public interface ILinesStore
{
    Task<IEnumerable<Line>> GetLines(LoggedInUser loggedInUser);
    Task<Line> CreateLine(LoggedInUser loggedInUser, string body, Guid? songId, bool forYesterday, Privacy privacy);
    Task<IEnumerable<DateTime>> GetLatestLineDates(LoggedInUser loggedInUser, int limit);
}