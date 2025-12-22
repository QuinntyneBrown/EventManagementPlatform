// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace EventManagementPlatform.Core.Services;

public interface IPasswordHasher
{
    (string hash, byte[] salt) HashPassword(string password);

    bool VerifyPassword(string password, string hash, byte[] salt);
}
