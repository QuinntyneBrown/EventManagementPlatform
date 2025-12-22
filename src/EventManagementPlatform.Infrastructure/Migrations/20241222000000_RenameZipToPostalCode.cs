// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementPlatform.Infrastructure.Migrations;

/// <inheritdoc />
public partial class RenameZipToPostalCode : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Rename Zip to PostalCode on Venues table
        migrationBuilder.RenameColumn(
            name: "Zip",
            table: "Venues",
            newName: "PostalCode");

        // Rename BillingZip to BillingPostalCode on Customers table
        migrationBuilder.RenameColumn(
            name: "BillingZip",
            table: "Customers",
            newName: "BillingPostalCode");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Revert PostalCode back to Zip on Venues table
        migrationBuilder.RenameColumn(
            name: "PostalCode",
            table: "Venues",
            newName: "Zip");

        // Revert BillingPostalCode back to BillingZip on Customers table
        migrationBuilder.RenameColumn(
            name: "BillingPostalCode",
            table: "Customers",
            newName: "BillingZip");
    }
}
