using Microsoft.EntityFrameworkCore.Migrations;

namespace IMASD.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    idDepartamento = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreDep = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.idDepartamento);
                });

            migrationBuilder.CreateTable(
                name: "Tabulador",
                columns: table => new
                {
                    idTabulador = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nivelTabulador = table.Column<string>(type: "varchar(1)", nullable: false),
                    sbruto = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tabulador", x => x.idTabulador);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    idEmpleado = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreEmp = table.Column<string>(type: "varchar(100)", nullable: false),
                    apellidosEmp = table.Column<string>(type: "varchar(100)", nullable: false),
                    direccionEmp = table.Column<string>(type: "varchar(100)", nullable: false),
                    telefonoEmp = table.Column<string>(nullable: true),
                    estatusEmp = table.Column<bool>(nullable: false),
                    idDepartamento = table.Column<int>(nullable: false),
                    idTabulador = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.idEmpleado);
                    table.ForeignKey(
                        name: "FK_Empleados_Departamentos_idDepartamento",
                        column: x => x.idDepartamento,
                        principalTable: "Departamentos",
                        principalColumn: "idDepartamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empleados_Tabulador_idTabulador",
                        column: x => x.idTabulador,
                        principalTable: "Tabulador",
                        principalColumn: "idTabulador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    idPago = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fPago = table.Column<string>(nullable: true),
                    idEmpleado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.idPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Empleados_idEmpleado",
                        column: x => x.idEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "idEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_idDepartamento",
                table: "Empleados",
                column: "idDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_idTabulador",
                table: "Empleados",
                column: "idTabulador");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_idEmpleado",
                table: "Pagos",
                column: "idEmpleado");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Tabulador");
        }
    }
}
